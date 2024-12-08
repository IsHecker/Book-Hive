namespace BookHive.Models;

public class ActivityLog
{
    public int Id { get; init; } // Unique identifier for the log entry
    public required int UserId { get; init; } // ID of the user who performed the action
    public required string Action { get; init; } // Description of the action (e.g., "Borrowed Book", "Returned Book")
    public required string BookName { get; init; }
    public required string? ActionDetails { get; init; }
    public DateTime Timestamp { get; init; } // Date and time of the action
}