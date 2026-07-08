using Microsoft.EntityFrameworkCore;
using UniversityEventManager.API.Models;

namespace UniversityEventManager.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<EventCategory> EventCategories => Set<EventCategory>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Registration> Registrations => Set<Registration>();
        public DbSet<Attendance> Attendances => Set<Attendance>();
        public DbSet<EventCreationRequest> EventCreationRequests => Set<EventCreationRequest>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ============================
            // Roles
            // ============================
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.HasKey(r => r.RoleID);

                entity.Property(r => r.RoleID)
                    .HasColumnName("roleID");

                entity.Property(r => r.RoleName)
                    .HasColumnName("roleName")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.HasIndex(r => r.RoleName)
                    .IsUnique();
            });

            // ============================
            // Users
            // ============================
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(u => u.UserID);

                entity.Property(u => u.UserID)
                    .HasColumnName("userID");

                entity.Property(u => u.FullName)
                    .HasColumnName("fullName")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(u => u.SchoolID)
                    .HasColumnName("schoolID")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(u => u.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(u => u.PasswordHash)
                    .HasColumnName("passwordHash")
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(u => u.Organisation)
                    .HasColumnName("organisation")
                    .HasMaxLength(150);

                entity.Property(u => u.RoleID)
                    .HasColumnName("roleID");

                entity.Property(u => u.AccountStatus)
                    .HasColumnName("accountStatus")
                    .HasMaxLength(50)
                    .HasDefaultValue("active")
                    .IsRequired();

                entity.Property(u => u.CreatedAt)
                    .HasColumnName("createdAt");

                entity.HasIndex(u => u.Email)
                    .IsUnique();

                entity.HasIndex(u => u.SchoolID)
                    .IsUnique();

                entity.HasOne<Role>()
                    .WithMany()
                    .HasForeignKey(u => u.RoleID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ============================
            // Event Categories
            // ============================
            modelBuilder.Entity<EventCategory>(entity =>
            {
                entity.ToTable("event_categories");

                entity.HasKey(c => c.CategoryID);

                entity.Property(c => c.CategoryID)
                    .HasColumnName("categoryID");

                entity.Property(c => c.CategoryName)
                    .HasColumnName("categoryName")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasIndex(c => c.CategoryName)
                    .IsUnique();
            });

            // ============================
            // Event Creation Requests
            // ============================
            modelBuilder.Entity<EventCreationRequest>(entity =>
            {
                entity.ToTable("event_creation_request");

                entity.HasKey(r => r.RequestID);

                entity.Property(r => r.RequestID)
                    .HasColumnName("requestID");

                entity.Property(r => r.SubmittedBy)
                    .HasColumnName("submittedBy");

                entity.Property(r => r.EventTitle)
                    .HasColumnName("eventTitle")
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(r => r.EventDescription)
                    .HasColumnName("eventDescription");

                entity.Property(r => r.Venue)
                    .HasColumnName("venue")
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(r => r.PosterURL)
                    .HasColumnName("posterURL")
                    .HasMaxLength(500);

                entity.Property(r => r.PosterS3Key)
                    .HasColumnName("posterS3Key")
                    .HasMaxLength(500);

                entity.Property(r => r.CategoryID)
                    .HasColumnName("categoryID");

                entity.Property(r => r.ProposedStartDatetime)
                    .HasColumnName("proposedStartDatetime");

                entity.Property(r => r.ProposedEndDatetime)
                    .HasColumnName("proposedEndDatetime");

                entity.Property(r => r.RequestStatus)
                    .HasColumnName("requestedStatus")
                    .HasMaxLength(50)
                    .HasDefaultValue("pending")
                    .IsRequired();

                entity.Property(r => r.SubmittedAt)
                    .HasColumnName("submittedAt");

                entity.Property(r => r.ReviewedBy)
                    .HasColumnName("reviewedBy");

                entity.Property(r => r.ReviewedAt)
                    .HasColumnName("reviewedAt");

                entity.Property(r => r.Remark)
                    .HasColumnName("remark");

                entity.Property(r => r.RequestCapacity)
                    .HasColumnName("requestCapacity");

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(r => r.SubmittedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(r => r.ReviewedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<EventCategory>()
                    .WithMany()
                    .HasForeignKey(r => r.CategoryID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ============================
            // Events
            // ============================
            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("events");

                entity.HasKey(e => e.EventID);

                entity.Property(e => e.EventID)
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
                    .HasColumnName("description");

                entity.Property(e => e.Venue)
                    .HasColumnName("venue")
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.PosterURL)
                    .HasColumnName("posterURL")
                    .HasMaxLength(500);

                entity.Property(e => e.PosterS3Key)
                    .HasColumnName("posterS3Key")
                    .HasMaxLength(500);

                entity.Property(e => e.StartDatetime)
                    .HasColumnName("startDatetime");

                entity.Property(e => e.EndDatetime)
                    .HasColumnName("endDatetime");

                entity.Property(e => e.Capacity)
                    .HasColumnName("capacity");

                entity.Property(e => e.EventStatus)
                    .HasColumnName("eventStatus")
                    .HasMaxLength(50)
                    .HasDefaultValue("upcoming")
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdAt");

                entity.HasIndex(e => e.CreationRequestID)
                    .IsUnique();

                entity.HasOne<EventCreationRequest>()
                    .WithMany()
                    .HasForeignKey(e => e.CreationRequestID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.OrganiserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.ApprovedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<EventCategory>()
                    .WithMany()
                    .HasForeignKey(e => e.CategoryID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ============================
            // Registrations
            // ============================
            modelBuilder.Entity<Registration>(entity =>
            {
                entity.ToTable("registrations");

                entity.HasKey(r => r.RegistrationID);

                entity.Property(r => r.RegistrationID)
                    .HasColumnName("registrationID");

                entity.Property(r => r.EventID)
                    .HasColumnName("eventID");

                entity.Property(r => r.UserID)
                    .HasColumnName("userID");

                entity.Property(r => r.QrCode)
                    .HasColumnName("qrCode")
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(r => r.RegistrationDate)
                    .HasColumnName("registrationDate");

                entity.Property(r => r.RegistrationStatus)
                    .HasColumnName("registrationStatus")
                    .HasMaxLength(50)
                    .HasDefaultValue("registered")
                    .IsRequired();

                entity.HasIndex(r => new { r.EventID, r.UserID })
                    .IsUnique();

                entity.HasIndex(r => r.QrCode)
                    .IsUnique();

                entity.HasOne<Event>()
                    .WithMany()
                    .HasForeignKey(r => r.EventID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(r => r.UserID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ============================
            // Attendance
            // ============================
            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.ToTable("attendance");

                entity.HasKey(a => a.AttendanceID);

                entity.Property(a => a.AttendanceID)
                    .HasColumnName("attendanceID");

                entity.Property(a => a.RegistrationID)
                    .HasColumnName("registrationID");

                entity.Property(a => a.CheckedBy)
                    .HasColumnName("checkedBy");

                entity.Property(a => a.AttendanceStatus)
                    .HasColumnName("attendanceStatus")
                    .HasMaxLength(50)
                    .HasDefaultValue("checked_in")
                    .IsRequired();

                entity.Property(a => a.CheckInDatetime)
                    .HasColumnName("checkInDatetime");

                entity.HasIndex(a => a.RegistrationID)
                    .IsUnique();

                entity.HasOne<Registration>()
                    .WithMany()
                    .HasForeignKey(a => a.RegistrationID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(a => a.CheckedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}