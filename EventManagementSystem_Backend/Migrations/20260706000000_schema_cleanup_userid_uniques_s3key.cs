using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagementSystem_Backend.Migrations
{
    /// <inheritdoc />
    public partial class schema_cleanup_userid_uniques_s3key : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "usersID",
                table: "users",
                newName: "userID");

            migrationBuilder.RenameColumn(
                name: "PosterS3Key",
                table: "events",
                newName: "posterS3Key");

            migrationBuilder.RenameColumn(
                name: "PosterS3Key",
                table: "event_creation_request",
                newName: "posterS3Key");

            migrationBuilder.AlterColumn<string>(
                name: "posterS3Key",
                table: "events",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "posterS3Key",
                table: "event_creation_request",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_registrations_eventID_userID",
                table: "registrations",
                columns: new[] { "eventID", "userID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_registrations_eventID_userID",
                table: "registrations");

            migrationBuilder.AlterColumn<string>(
                name: "posterS3Key",
                table: "events",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "posterS3Key",
                table: "event_creation_request",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.RenameColumn(
                name: "posterS3Key",
                table: "events",
                newName: "PosterS3Key");

            migrationBuilder.RenameColumn(
                name: "posterS3Key",
                table: "event_creation_request",
                newName: "PosterS3Key");

            migrationBuilder.RenameColumn(
                name: "userID",
                table: "users",
                newName: "usersID");
        }
    }
}
