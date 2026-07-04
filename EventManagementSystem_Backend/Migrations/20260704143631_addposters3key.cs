using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagementSystem_Backend.Migrations
{
    /// <inheritdoc />
    public partial class addposters3key : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PosterS3Key",
                table: "events",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PosterS3Key",
                table: "events");
        }
    }
}
