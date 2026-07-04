using EventManagementSystem_Backend.Models.Enums;
using System.Globalization;

namespace EventManagementSystem_Backend.Models;

public class Event
{
    public long EventId { get; set; }

    public long CreationRequestID { get; set; }
    public long OrganiserID { get; set; }
    public long ApprovedBy { get; set; }
    public long CategoryID { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Venue { get; set; } = string.Empty;
    public string? PosterURL { get; set; }
    public string? PosterS3Key { get; set; }

    public DateTime StartDatetime { get; set; }
    public DateTime EndDatetime { get; set; }

    public int Capacity { get; set; }
    public EventStatus EventStatus { get; set; } = EventStatus.UPCOMING;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public EventCreationRequest? CreationRequest { get; set; }
    public User? Organiser { get; set; }
    public User? Approver { get; set; }
    public EventCategory? Category { get; set; }

    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
