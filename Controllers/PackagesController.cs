using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackageTrackingAPI.Data;
using PackageTrackingAPI.Models;
using PackageTrackingAPI.Services;

namespace PackageTrackingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PackagesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IStatusTransitionService _transitions;

    public PackagesController(AppDbContext context, IStatusTransitionService transitions)
    {
        _context = context;
        _transitions = transitions;
    }

    // GET: api/packages
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Package>>> GetPackages()
    {
        return await _context.Packages
            .Include(p => p.History)
            .OrderByDescending(p => p.Id)
            .ToListAsync();
    }

    // GET: api/packages/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Package>> GetPackage(int id)
    {
        var package = await _context.Packages
            .Include(p => p.History)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (package == null)
            return NotFound();

        return package;
    }

    // OPTIONAL: GET: api/packages/track/ABC12345

    [HttpGet("track/{trackingNumber}")]
    public async Task<ActionResult<Package>> GetByTrackingNumber(string trackingNumber)
    {
        var package = await _context.Packages
            .Include(p => p.History)
            .FirstOrDefaultAsync(p => p.TrackingNumber == trackingNumber);

        if (package == null)
            return NotFound();

        return package;
    }

    // POST: api/packages
    [HttpPost]
    public async Task<ActionResult<Package>> CreatePackage(Package package)
    {

        package.TrackingNumber = Guid.NewGuid().ToString("N")[..8].ToUpper();


        package.Status = PackageStatus.Created;
        package.CreatedAt = DateTime.UtcNow;
        package.History ??= new List<StatusHistory>();
        package.History.Add(new StatusHistory
        {
            Status = PackageStatus.Created,
            ChangedAt = DateTime.UtcNow
        });

        _context.Packages.Add(package);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPackage), new { id = package.Id }, package);
    }

    // NEW: GET: api/packages/{id}/allowed-statuses

    [HttpGet("{id:int}/allowed-statuses")]
    public async Task<ActionResult<IEnumerable<PackageStatus>>> GetAllowedStatuses(int id)
    {
        var package = await _context.Packages.FindAsync(id);
        if (package == null)
            return NotFound();

        var allowed = _transitions.GetAllowedTargets(package.Status);
        return Ok(allowed);
    }

    // NEW: PUT: api/packages/{id}/status

    [HttpPut("{id:int}/status")]
    public async Task<ActionResult<Package>> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
    {
        var package = await _context.Packages
            .Include(p => p.History)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (package == null)
            return NotFound($"Package with id {id} not found.");

        if (package.Status == request.NewStatus)
            return BadRequest("New status is the same as current.");

        if (_transitions.IsFinalStatus(package.Status))
            return BadRequest($"Status '{package.Status}' is final and cannot be changed.");

        if (!_transitions.CanTransition(package.Status, request.NewStatus))
            return BadRequest($"Cannot change status from '{package.Status}' to '{request.NewStatus}'.");


        package.Status = request.NewStatus;
        package.History.Add(new StatusHistory
        {
            Status = request.NewStatus,
            ChangedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
        return Ok(package);
    }
}
