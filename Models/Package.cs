namespace PackageTrackingAPI.Models;

public enum PackageStatus
{
    Created,
    Sent,
    Accepted,
    Returned,
    Canceled
}

public class Package
{
    public int Id { get; set; }
    public string TrackingNumber { get; set; } = string.Empty;

    // Sender
    public string SenderName { get; set; } = string.Empty;
    public string SenderAddress { get; set; } = string.Empty;
    public string SenderPhone { get; set; } = string.Empty;

    // Recipient
    public string RecipientName { get; set; } = string.Empty;
    public string RecipientAddress { get; set; } = string.Empty;
    public string RecipientPhone { get; set; } = string.Empty;

    // Status
    public PackageStatus Status { get; set; } = PackageStatus.Created;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // History
    public List<StatusHistory> History { get; set; } = new();
}

public class StatusHistory
{
    public int Id { get; set; }
    public PackageStatus Status { get; set; }
    public DateTime ChangedAt { get; set; }
}
