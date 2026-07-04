namespace EventManagementSystem_Backend.Models;

public class EventCategory
{
    public long CategoryID { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<EventCreationRequest> EventCreationRequests { get; set; } = new List<EventCreationRequest>();
}
