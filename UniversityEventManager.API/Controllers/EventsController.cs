using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Security.Claims;
using UniversityEventManager.API.Data;
using UniversityEventManager.API.Models;

namespace UniversityEventManager.API.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/events
        // Supports:
        // /api/events
        // /api/events?categoryID=1
        // /api/events?dateFrom=2026-07-01T00:00:00&dateTo=2026-07-31T23:59:59
        [HttpGet]
        public async Task<IActionResult> GetEvents(
            [FromQuery] long? categoryID,
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo)
        {
            long? currentUserId = GetCurrentUserId();

            var query = _context.Events
                .Where(e => e.EventStatus == "upcoming" || e.EventStatus == "active");

            if (categoryID.HasValue)
            {
                query = query.Where(e => e.CategoryID == categoryID.Value);
            }

            if (dateFrom.HasValue)
            {
                query = query.Where(e => e.StartDatetime >= dateFrom.Value);
            }

            if (dateTo.HasValue)
            {
                query = query.Where(e => e.StartDatetime <= dateTo.Value);
            }

            var events = await query
                .Select(e => new
                {
                    eventID = e.EventID,
                    title = e.Title,
                    description = e.Description ?? "",
                    venue = e.Venue,
                    posterURL = e.PosterURL ?? "",
                    startDatetime = e.StartDatetime,
                    endDatetime = e.EndDatetime,
                    capacity = e.Capacity,

                    registeredCount = _context.Registrations
                        .Count(r => r.EventID == e.EventID &&
                                    r.RegistrationStatus == "registered"),

                    availableSeats = e.Capacity - _context.Registrations
                        .Count(r => r.EventID == e.EventID &&
                                    r.RegistrationStatus == "registered"),

                    categoryName = _context.EventCategories
                        .Where(c => c.CategoryID == e.CategoryID)
                        .Select(c => c.CategoryName)
                        .FirstOrDefault() ?? "",

                    eventStatus = e.EventStatus,

                    isRegisteredByCurrentUser = currentUserId.HasValue &&
                        _context.Registrations.Any(r =>
                            r.EventID == e.EventID &&
                            r.UserID == currentUserId.Value &&
                            r.RegistrationStatus == "registered")
                })
                .OrderByDescending(e => e.registeredCount)
                .ToListAsync();

            return Ok(events);
        }

        // GET: api/events/1
        [HttpGet("{eventID}")]
        public async Task<IActionResult> GetEventById(long eventID)
        {
            long? currentUserId = GetCurrentUserId();

            var eventItem = await _context.Events
                .Where(e => e.EventID == eventID)
                .Select(e => new
                {
                    eventID = e.EventID,
                    title = e.Title,
                    description = e.Description ?? "",
                    venue = e.Venue,
                    posterURL = e.PosterURL ?? "",
                    startDatetime = e.StartDatetime,
                    endDatetime = e.EndDatetime,
                    capacity = e.Capacity,

                    registeredCount = _context.Registrations
                        .Count(r => r.EventID == e.EventID &&
                                    r.RegistrationStatus == "registered"),

                    availableSeats = e.Capacity - _context.Registrations
                        .Count(r => r.EventID == e.EventID &&
                                    r.RegistrationStatus == "registered"),

                    categoryName = _context.EventCategories
                        .Where(c => c.CategoryID == e.CategoryID)
                        .Select(c => c.CategoryName)
                        .FirstOrDefault() ?? "",

                    eventStatus = e.EventStatus,

                    isRegisteredByCurrentUser = currentUserId.HasValue &&
                        _context.Registrations.Any(r =>
                            r.EventID == e.EventID &&
                            r.UserID == currentUserId.Value &&
                            r.RegistrationStatus == "registered")
                })
                .FirstOrDefaultAsync();

            if (eventItem == null)
            {
                return NotFound(new { message = "Event not found" });
            }

            return Ok(eventItem);
        }

        // POST: api/events/1/register
        [Authorize]
        [HttpPost("{eventID}/register")]
        public async Task<IActionResult> RegisterEvent(long eventID)
        {
            var userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdValue))
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            long userId = long.Parse(userIdValue);

            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == eventID);

            if (eventItem == null)
            {
                return NotFound(new { message = "Event not found" });
            }

            if (eventItem.EventStatus != "upcoming" && eventItem.EventStatus != "active")
            {
                return BadRequest(new { message = "Event is not open for registration" });
            }

            var existingRegistration = await _context.Registrations
                .FirstOrDefaultAsync(r =>
                    r.EventID == eventID &&
                    r.UserID == userId &&
                    r.RegistrationStatus == "registered");

            if (existingRegistration != null)
            {
                var existingTicketResponse = await BuildTicketResponse(existingRegistration.RegistrationID, userId);

                return Ok(existingTicketResponse);
            }

            int registeredCount = await _context.Registrations
                .CountAsync(r => r.EventID == eventID &&
                                 r.RegistrationStatus == "registered");

            if (registeredCount >= eventItem.Capacity)
            {
                return BadRequest(new { message = "Event is full" });
            }

            var registration = new Registration
            {
                EventID = eventID,
                UserID = userId,
                RegistrationDate = DateTime.Now,
                RegistrationStatus = "registered",
                QrCode = Guid.NewGuid().ToString("N")
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            var ticketResponse = await BuildTicketResponse(registration.RegistrationID, userId);

            return Ok(ticketResponse);
        }

        private long? GetCurrentUserId()
        {
            var userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdValue))
            {
                return null;
            }

            if (long.TryParse(userIdValue, out long userId))
            {
                return userId;
            }

            return null;
        }

        private async Task<object?> BuildTicketResponse(long registrationID, long userId)
        {
            var ticket = await _context.Registrations
                .Where(r => r.RegistrationID == registrationID)
                .Select(r => new
                {
                    registrationID = r.RegistrationID,
                    userID = r.UserID,
                    registrationStatus = r.RegistrationStatus,
                    qrCode = r.QrCode,

                    attendanceStatus = _context.Attendances
                        .Where(a => a.RegistrationID == r.RegistrationID)
                        .Select(a => a.AttendanceStatus)
                        .FirstOrDefault() ?? "not_checked_in",

                    @event = _context.Events
                        .Where(e => e.EventID == r.EventID)
                        .Select(e => new
                        {
                            eventID = e.EventID,
                            title = e.Title,
                            description = e.Description ?? "",
                            venue = e.Venue,
                            posterURL = e.PosterURL ?? "",
                            startDatetime = e.StartDatetime,
                            endDatetime = e.EndDatetime,
                            capacity = e.Capacity,

                            registeredCount = _context.Registrations
                                .Count(reg => reg.EventID == e.EventID &&
                                              reg.RegistrationStatus == "registered"),

                            availableSeats = e.Capacity - _context.Registrations
                                .Count(reg => reg.EventID == e.EventID &&
                                              reg.RegistrationStatus == "registered"),

                            categoryName = _context.EventCategories
                                .Where(c => c.CategoryID == e.CategoryID)
                                .Select(c => c.CategoryName)
                                .FirstOrDefault() ?? "",

                            eventStatus = e.EventStatus,

                            isRegisteredByCurrentUser = _context.Registrations
                                .Any(reg =>
                                    reg.EventID == e.EventID &&
                                    reg.UserID == userId &&
                                    reg.RegistrationStatus == "registered")
                        })
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

return ticket;
        }
    }
}