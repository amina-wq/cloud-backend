using EventManagementSystem_Backend.Models.Enums;

namespace EventManagementSystem_Backend.Models;

public class User
{
    public long UserId { get; set; }
    public int RoleId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string SchoolID { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Organisation { get; set; }
    public AccountStatus AccountStatus { get; set; } = AccountStatus.ACTIVE;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Role? Role { get; set; }

    public ICollection<EventCreationRequest> SubmittedRequests { get; set; } = new List<EventCreationRequest>();
    public ICollection<EventCreationRequest> ReviewedRequests { get; set; } = new List<EventCreationRequest>();
    public ICollection<Event> OrganisedEvents { get; set; } = new List<Event>();
    public ICollection<Event> ApprovedEvents { get; set; } = new List<Event>();
    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    public ICollection<Attendance> CheckedAttendances { get; set; } = new List<Attendance>();
}
