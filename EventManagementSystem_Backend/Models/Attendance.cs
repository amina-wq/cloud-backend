using EventManagementSystem_Backend.Models.Enums;

namespace EventManagementSystem_Backend.Models;

public class Attendance
{
    public long AttendanceId { get; set; }
    public long RegistrationId { get; set; }
    public long CheckedBy { get; set; }

    public AttendanceStatus AttendanceStatus { get; set; } = AttendanceStatus.ABSENT;
    public DateTime CheckInDatetime { get; set; } = DateTime.UtcNow;

    public Registration? Registration { get; set; }
    public User? CheckedByUser { get; set; }
}
