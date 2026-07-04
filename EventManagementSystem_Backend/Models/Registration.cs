using EventManagementSystem_Backend.Models.Enums;

namespace EventManagementSystem_Backend.Models;

public class Registration
{
    public long RegistrationId { get; set; }
    public long EventId { get; set; }
    public long UserId { get; set; }
    
    public string QrCode { get; set; } = string.Empty;
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    public RegistrationStatus RegistrationStatus { get; set; } = RegistrationStatus.UNREGISTERED;

    public Event? Event { get; set; }
    public User? User { get; set; }
    public Attendance? Attendance { get; set; }
}
