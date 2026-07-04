using EventManagementSystem_Backend.Models.Enums;

namespace EventManagementSystem_Backend.Models;

public class EventCreationRequest
{
    public long RequestID { get; set; }
    public long SubmittedBy { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public string? EventDescription { get; set; }
    public string Venue { get; set; } = string.Empty;
    public string? PosterURL { get; set; }
    public string? PosterS3Key { get; set; }

    public long CategoryID { get; set; }

    public DateTime ProposedStartDatetime { get; set; }
    public DateTime ProposedEndDatetime { get; set; }

    public RequestedStatus RequestedStatus { get; set; } = RequestedStatus.PENDING;
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    public long? ReviewedBy { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? Remark { get; set; }

    public int RequestCapacity { get; set; }

    public User? SubmittedByUser { get; set; }
    public User? ReviewedByUser { get; set; }
    public EventCategory? Category { get; set; }
    public Event? Event { get; set; }
}
