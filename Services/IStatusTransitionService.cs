using PackageTrackingAPI.Models;

namespace PackageTrackingAPI.Services;

public interface IStatusTransitionService
{
    bool IsFinalStatus(PackageStatus status);
    bool CanTransition(PackageStatus from, PackageStatus to);
    IReadOnlyCollection<PackageStatus> GetAllowedTargets(PackageStatus from);
}
