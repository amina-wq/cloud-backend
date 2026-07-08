using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using UniversityEventManager.API.Data;
using UniversityEventManager.API.Models;
using UniversityEventManager.API.Services;

namespace UniversityEventManager.API.Controllers
{
    [ApiController]
    [Route("api/event-requests")]
    [Authorize]
    public class EventRequestsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly S3StorageService _s3StorageService;

        public EventRequestsController(AppDbContext context, S3StorageService s3StorageService)
        {
            _context = context;
            _s3StorageService = s3StorageService;
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyEventRequests()
        {
            var userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdValue))
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            long userId = long.Parse(userIdValue);

            var requests = await _context.EventCreationRequests
                .Where(r => r.SubmittedBy == userId)
                .Select(r => new
                {
                    requestID = r.RequestID,
                    eventTitle = r.EventTitle,
                    eventDescription = r.EventDescription,
                    venue = r.Venue,
                    posterURL = r.PosterURL,
                    categoryName = _context.EventCategories
                        .Where(c => c.CategoryID == r.CategoryID)
                        .Select(c => c.CategoryName)
                        .FirstOrDefault(),
                    proposedStartDatetime = r.ProposedStartDatetime,
                    proposedEndDatetime = r.ProposedEndDatetime,
                    requestCapacity = r.RequestCapacity,
                    requestStatus = r.RequestStatus,
                    submittedAt = r.SubmittedAt,
                    reviewedAt = r.ReviewedAt,
                    remark = r.Remark,
                    submittedByName = _context.Users
                        .Where(u => u.UserID == r.SubmittedBy)
                        .Select(u => u.FullName)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(requests);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEventRequest(CreateEventRequestDto request)
        {
            return await CreateEventRequestInternal(
                request.EventTitle,
                request.EventDescription,
                request.Venue,
                request.CategoryID,
                request.ProposedStartDatetime,
                request.ProposedEndDatetime,
                request.RequestCapacity,
                null
            );
        }

        [HttpPost("with-poster")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateEventRequestWithPoster([FromForm] CreateEventRequestWithPosterDto request)
        {
            return await CreateEventRequestInternal(
                request.EventTitle,
                request.EventDescription,
                request.Venue,
                request.CategoryID,
                request.ProposedStartDatetime,
                request.ProposedEndDatetime,
                request.RequestCapacity,
                request.PosterFile
            );
        }

        private async Task<IActionResult> CreateEventRequestInternal(
            string eventTitle,
            string? eventDescription,
            string venue,
            long categoryID,
            DateTime proposedStartDatetime,
            DateTime proposedEndDatetime,
            int requestCapacity,
            IFormFile? posterFile)
        {
            var userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdValue))
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            long userId = long.Parse(userIdValue);

            if (proposedEndDatetime <= proposedStartDatetime)
            {
                return BadRequest(new { message = "End datetime must be after start datetime" });
            }

            if (requestCapacity <= 0)
            {
                return BadRequest(new { message = "Capacity must be greater than 0" });
            }

            var categoryExists = await _context.EventCategories
                .AnyAsync(c => c.CategoryID == categoryID);

            if (!categoryExists)
            {
                return BadRequest(new { message = "Invalid category" });
            }

            string posterUrl = "";
            string? posterS3Key = null;

            if (posterFile != null && posterFile.Length > 0)
            {
                try
                {
                    var uploadResult = await _s3StorageService.UploadEventPosterAsync(posterFile);
                    posterUrl = uploadResult.PosterUrl;
                    posterS3Key = uploadResult.PosterS3Key;
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            var eventRequest = new EventCreationRequest
            {
                EventTitle = eventTitle,
                EventDescription = eventDescription,
                Venue = venue,
                CategoryID = categoryID,
                ProposedStartDatetime = proposedStartDatetime,
                ProposedEndDatetime = proposedEndDatetime,
                RequestCapacity = requestCapacity,
                RequestStatus = "pending",
                SubmittedBy = userId,
                SubmittedAt = DateTime.Now,
                ReviewedBy = null,
                ReviewedAt = null,
                Remark = null,
                PosterURL = posterUrl,
                PosterS3Key = posterS3Key
            };

            _context.EventCreationRequests.Add(eventRequest);
            await _context.SaveChangesAsync();

            var categoryName = await _context.EventCategories
                .Where(c => c.CategoryID == eventRequest.CategoryID)
                .Select(c => c.CategoryName)
                .FirstOrDefaultAsync();

            var userName = await _context.Users
                .Where(u => u.UserID == userId)
                .Select(u => u.FullName)
                .FirstOrDefaultAsync();

            return Ok(new
            {
                requestID = eventRequest.RequestID,
                eventTitle = eventRequest.EventTitle,
                eventDescription = eventRequest.EventDescription,
                venue = eventRequest.Venue,
                posterURL = eventRequest.PosterURL,
                categoryName,
                proposedStartDatetime = eventRequest.ProposedStartDatetime,
                proposedEndDatetime = eventRequest.ProposedEndDatetime,
                requestCapacity = eventRequest.RequestCapacity,
                requestStatus = eventRequest.RequestStatus,
                submittedAt = eventRequest.SubmittedAt,
                reviewedAt = eventRequest.ReviewedAt,
                remark = eventRequest.Remark,
                submittedByName = userName
            });
        }
    }

    public class CreateEventRequestDto
    {
        public string EventTitle { get; set; } = string.Empty;
        public string? EventDescription { get; set; }
        public string Venue { get; set; } = string.Empty;
        public long CategoryID { get; set; }
        public DateTime ProposedStartDatetime { get; set; }
        public DateTime ProposedEndDatetime { get; set; }
        public int RequestCapacity { get; set; }
    }

    public class CreateEventRequestWithPosterDto
    {
        [Required]
        public string EventTitle { get; set; } = string.Empty;

        public string? EventDescription { get; set; }

        [Required]
        public string Venue { get; set; } = string.Empty;

        public long CategoryID { get; set; }

        public DateTime ProposedStartDatetime { get; set; }

        public DateTime ProposedEndDatetime { get; set; }

        public int RequestCapacity { get; set; }

        public IFormFile? PosterFile { get; set; }
    }
}