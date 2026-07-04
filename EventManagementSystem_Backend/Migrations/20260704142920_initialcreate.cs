using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagementSystem_Backend.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "event_categories",
                columns: table => new
                {
                    categoryID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    categoryName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_categories", x => x.categoryID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    roleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    roleName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.roleID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    usersID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    roleID = table.Column<int>(type: "int", nullable: false),
                    fullName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    schoolID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    passwordHash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    organisation = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    accountStatus = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    createdAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.usersID);
                    table.ForeignKey(
                        name: "FK_users_roles_roleID",
                        column: x => x.roleID,
                        principalTable: "roles",
                        principalColumn: "roleID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "event_creation_request",
                columns: table => new
                {
                    requestID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    submittedBy = table.Column<long>(type: "bigint", nullable: false),
                    eventTitle = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    eventDescription = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    venue = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    posterURL = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PosterS3Key = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    categoryID = table.Column<long>(type: "bigint", nullable: false),
                    proposedStartDatetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    proposedEndDatetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    requestedStatus = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    submittedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    reviewedBy = table.Column<long>(type: "bigint", nullable: true),
                    reviewedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    remark = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    requestCapacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_creation_request", x => x.requestID);
                    table.ForeignKey(
                        name: "FK_event_creation_request_event_categories_categoryID",
                        column: x => x.categoryID,
                        principalTable: "event_categories",
                        principalColumn: "categoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_event_creation_request_users_reviewedBy",
                        column: x => x.reviewedBy,
                        principalTable: "users",
                        principalColumn: "usersID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_event_creation_request_users_submittedBy",
                        column: x => x.submittedBy,
                        principalTable: "users",
                        principalColumn: "usersID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    eventID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    creationRequestID = table.Column<long>(type: "bigint", nullable: false),
                    organiserID = table.Column<long>(type: "bigint", nullable: false),
                    approvedBy = table.Column<long>(type: "bigint", nullable: false),
                    categoryID = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    venue = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    posterURL = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    startDatetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    endDatetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    capacity = table.Column<int>(type: "int", nullable: false),
                    eventStatus = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    createdAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.eventID);
                    table.ForeignKey(
                        name: "FK_events_event_categories_categoryID",
                        column: x => x.categoryID,
                        principalTable: "event_categories",
                        principalColumn: "categoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_events_event_creation_request_creationRequestID",
                        column: x => x.creationRequestID,
                        principalTable: "event_creation_request",
                        principalColumn: "requestID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_events_users_approvedBy",
                        column: x => x.approvedBy,
                        principalTable: "users",
                        principalColumn: "usersID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_events_users_organiserID",
                        column: x => x.organiserID,
                        principalTable: "users",
                        principalColumn: "usersID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "registrations",
                columns: table => new
                {
                    registrationID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    eventID = table.Column<long>(type: "bigint", nullable: false),
                    userID = table.Column<long>(type: "bigint", nullable: false),
                    qrCode = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    registrationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    registrationStatus = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_registrations", x => x.registrationID);
                    table.ForeignKey(
                        name: "FK_registrations_events_eventID",
                        column: x => x.eventID,
                        principalTable: "events",
                        principalColumn: "eventID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_registrations_users_userID",
                        column: x => x.userID,
                        principalTable: "users",
                        principalColumn: "usersID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "attendance",
                columns: table => new
                {
                    attendanceID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    registrationID = table.Column<long>(type: "bigint", nullable: false),
                    checkedBy = table.Column<long>(type: "bigint", nullable: false),
                    attendanceStatus = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    checkInDatetime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendance", x => x.attendanceID);
                    table.ForeignKey(
                        name: "FK_attendance_registrations_registrationID",
                        column: x => x.registrationID,
                        principalTable: "registrations",
                        principalColumn: "registrationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_attendance_users_checkedBy",
                        column: x => x.checkedBy,
                        principalTable: "users",
                        principalColumn: "usersID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_attendance_checkedBy",
                table: "attendance",
                column: "checkedBy");

            migrationBuilder.CreateIndex(
                name: "IX_attendance_registrationID",
                table: "attendance",
                column: "registrationID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_event_categories_categoryName",
                table: "event_categories",
                column: "categoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_event_creation_request_categoryID",
                table: "event_creation_request",
                column: "categoryID");

            migrationBuilder.CreateIndex(
                name: "IX_event_creation_request_reviewedBy",
                table: "event_creation_request",
                column: "reviewedBy");

            migrationBuilder.CreateIndex(
                name: "IX_event_creation_request_submittedBy",
                table: "event_creation_request",
                column: "submittedBy");

            migrationBuilder.CreateIndex(
                name: "IX_events_approvedBy",
                table: "events",
                column: "approvedBy");

            migrationBuilder.CreateIndex(
                name: "IX_events_categoryID",
                table: "events",
                column: "categoryID");

            migrationBuilder.CreateIndex(
                name: "IX_events_creationRequestID",
                table: "events",
                column: "creationRequestID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_events_organiserID",
                table: "events",
                column: "organiserID");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_eventID",
                table: "registrations",
                column: "eventID");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_qrCode",
                table: "registrations",
                column: "qrCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_registrations_userID",
                table: "registrations",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_roles_roleName",
                table: "roles",
                column: "roleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_roleID",
                table: "users",
                column: "roleID");

            migrationBuilder.CreateIndex(
                name: "IX_users_schoolID",
                table: "users",
                column: "schoolID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attendance");

            migrationBuilder.DropTable(
                name: "registrations");

            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.DropTable(
                name: "event_creation_request");

            migrationBuilder.DropTable(
                name: "event_categories");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
