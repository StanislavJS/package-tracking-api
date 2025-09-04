using PackageTrackingAPI.Models;

namespace PackageTrackingAPI.Services;

public class StatusTransitionService : IStatusTransitionService
{

    private static readonly Dictionary<PackageStatus, PackageStatus[]> Map = new()
    {
        [PackageStatus.Created] = new[] { PackageStatus.Sent, PackageStatus.Canceled },
        [PackageStatus.Sent] = new[] { PackageStatus.Accepted, PackageStatus.Returned, PackageStatus.Canceled },
        [PackageStatus.Returned] = new[] { PackageStatus.Sent, PackageStatus.Canceled },
        [PackageStatus.Accepted] = Array.Empty<PackageStatus>(),
        [PackageStatus.Canceled] = Array.Empty<PackageStatus>()
    };

    public bool IsFinalStatus(PackageStatus status)
        => status is PackageStatus.Accepted or PackageStatus.Canceled;

    public bool CanTransition(PackageStatus from, PackageStatus to)
        => Map.TryGetValue(from, out var allowed) && allowed.Contains(to);

    public IReadOnlyCollection<PackageStatus> GetAllowedTargets(PackageStatus from)
        => Map.TryGetValue(from, out var allowed) ? allowed : Array.Empty<PackageStatus>();
}
