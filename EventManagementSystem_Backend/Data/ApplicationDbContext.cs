using EventManagementSystem_Backend.Models;
using EventManagementSystem_Backend.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem_Backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<EventCreationRequest> EventCreationRequests => Set<EventCreationRequest>();
    public DbSet<EventCategory> EventCategories => Set<EventCategory>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Registration> Registrations => Set<Registration>();
    public DbSet<Attendance> Attendances => Set<Attendance>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("roles");

            entity.HasKey(e => e.RoleId);

            entity.Property(e => e.RoleId)
                .HasColumnName("roleID");

            entity.Property(e => e.RoleName)
                .HasColumnName("roleName")
                .HasMaxLength(50)
                .IsRequired();

            entity.HasIndex(e => e.RoleName)
                .IsUnique();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(e => e.UserId);

            entity.Property(e => e.UserId)
                .HasColumnName("usersID");

            entity.Property(e => e.RoleId)
                .HasColumnName("roleID");

            entity.Property(e => e.FullName)
                .HasColumnName("fullName")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.SchoolID)
                .HasColumnName("schoolID")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Email)
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.PasswordHash)
                .HasColumnName("passwordHash")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.Organisation)
                .HasColumnName("organisation")
                .HasMaxLength(150);

            entity.Property(e => e.AccountStatus)
                .HasColumnName("accountStatus")
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(e => e.CreatedAt)
                .HasColumnName("createdAt");

            entity.HasIndex(e => e.SchoolID)
                .IsUnique();

            entity.HasIndex(e => e.Email)
                .IsUnique();

            entity.HasOne(e => e.Role)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<EventCategory>(entity =>
        {
            entity.ToTable("event_categories");

            entity.HasKey(e => e.CategoryID);

            entity.Property(e => e.CategoryID)
                .HasColumnName("categoryID");

            entity.Property(e => e.CategoryName)
                .HasColumnName("categoryName")
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(e => e.CategoryName)
                .IsUnique();
        });

        modelBuilder.Entity<EventCreationRequest>(entity =>
        {
            entity.ToTable("event_creation_request");

            entity.HasKey(e => e.RequestID);

            entity.Property(e => e.RequestID)
                .HasColumnName("requestID");

            entity.Property(e => e.SubmittedBy)
                .HasColumnName("submittedBy");

            entity.Property(e => e.EventTitle)
                .HasColumnName("eventTitle")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.EventDescription)
                .HasColumnName("eventDescription")
                .HasColumnType("text");

            entity.Property(e => e.Venue)
                .HasColumnName("venue")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.PosterURL)
                .HasColumnName("posterURL")
                .HasMaxLength(500);

            entity.Property(e => e.CategoryID)
                .HasColumnName("categoryID");

            entity.Property(e => e.ProposedStartDatetime)
                .HasColumnName("proposedStartDatetime");

            entity.Property(e => e.ProposedEndDatetime)
                .HasColumnName("proposedEndDatetime");

            entity.Property(e => e.RequestedStatus)
                .HasColumnName("requestedStatus")
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(e => e.SubmittedAt)
                .HasColumnName("submittedAt");

            entity.Property(e => e.ReviewedBy)
                .HasColumnName("reviewedBy");

            entity.Property(e => e.ReviewedAt)
                .HasColumnName("reviewedAt");

            entity.Property(e => e.Remark)
                .HasColumnName("remark");

            entity.Property(e => e.RequestCapacity)
                .HasColumnName("requestCapacity");

            entity.HasOne(e => e.SubmittedByUser)
                .WithMany(e => e.SubmittedRequests)
                .HasForeignKey(e => e.SubmittedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ReviewedByUser)
                .WithMany(e => e.ReviewedRequests)
                .HasForeignKey(e => e.ReviewedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Category)
                .WithMany(e => e.EventCreationRequests)
                .HasForeignKey(e => e.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("events");

            entity.HasKey(e => e.EventId);

            entity.Property(e => e.EventId)
                .HasColumnName("eventID");

            entity.Property(e => e.CreationRequestID)
                .HasColumnName("creationRequestID");

            entity.Property(e => e.OrganiserID)
                .HasColumnName("organiserID");

            entity.Property(e => e.ApprovedBy)
                .HasColumnName("approvedBy");

            entity.Property(e => e.CategoryID)
                .HasColumnName("categoryID");

            entity.Property(e => e.Title)
                .HasColumnName("title")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasColumnType("text");

            entity.Property(e => e.Venue)
                .HasColumnName("venue")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.PosterURL)
                .HasColumnName("posterURL")
                .HasMaxLength(500);

            entity.Property(e => e.StartDatetime)
                .HasColumnName("startDatetime");

            entity.Property(e => e.EndDatetime)
                .HasColumnName("endDatetime");

            entity.Property(e => e.Capacity)
                .HasColumnName("capacity");

            entity.Property(e => e.EventStatus)
                .HasColumnName("eventStatus")
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(e => e.CreatedAt)
                .HasColumnName("createdAt");

            entity.HasIndex(e => e.CreationRequestID)
                .IsUnique();

            entity.HasOne(e => e.CreationRequest)
                .WithOne(e => e.Event)
                .HasForeignKey<Event>(e => e.CreationRequestID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Organiser)
                .WithMany(e => e.OrganisedEvents)
                .HasForeignKey(e => e.OrganiserID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Approver)
                .WithMany(e => e.ApprovedEvents)
                .HasForeignKey(e => e.ApprovedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Category)
                .WithMany(e => e.Events)
                .HasForeignKey(e => e.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.ToTable("registrations");

            entity.HasKey(e => e.RegistrationId);

            entity.Property(e => e.RegistrationId)
                .HasColumnName("registrationID");

            entity.Property(e => e.EventId)
                .HasColumnName("eventID");

            entity.Property(e => e.UserId)
                .HasColumnName("userID");

            entity.Property(e => e.QrCode)
                .HasColumnName("qrCode")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.RegistrationDate)
                .HasColumnName("registrationDate");

            entity.Property(e => e.RegistrationStatus)
                .HasColumnName("registrationStatus")
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.HasIndex(e => e.QrCode)
                .IsUnique();

            entity.HasOne(e => e.Event)
                .WithMany(e => e.Registrations)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.User)
                .WithMany(e => e.Registrations)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.ToTable("attendance");

            entity.HasKey(e => e.AttendanceId);

            entity.Property(e => e.AttendanceId)
                .HasColumnName("attendanceID");

            entity.Property(e => e.RegistrationId)
                .HasColumnName("registrationID");

            entity.Property(e => e.CheckedBy)
                .HasColumnName("checkedBy");

            entity.Property(e => e.AttendanceStatus)
                .HasColumnName("attendanceStatus")
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(e => e.CheckInDatetime)
                .HasColumnName("checkInDatetime");

            entity.HasOne(e => e.Registration)
                .WithOne(e => e.Attendance)
                .HasForeignKey<Attendance>(e => e.RegistrationId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CheckedByUser)
                .WithMany(e => e.CheckedAttendances)
                .HasForeignKey(e => e.CheckedBy)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
