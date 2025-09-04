using Microsoft.EntityFrameworkCore;
using PackageTrackingAPI.Models;

namespace PackageTrackingAPI.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Package> Packages => Set<Package>();
    public DbSet<StatusHistory> StatusHistories => Set<StatusHistory>();
}
