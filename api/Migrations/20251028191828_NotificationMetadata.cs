using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientPortalApi.Migrations
{
    /// <inheritdoc />
    public partial class NotificationMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Metadata_ProjectId",
                table: "Notifications",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Metadata_ResourceId",
                table: "Notifications",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Metadata_ResourceType",
                table: "Notifications",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Metadata_TaskId",
                table: "Notifications",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_Users_UserId",
                table: "ProjectMembers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectMembers_UserId",
                table: "ProjectMembers");

            migrationBuilder.DropColumn(
                name: "Metadata_ProjectId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Metadata_ResourceId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Metadata_ResourceType",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Metadata_TaskId",
                table: "Notifications");
        }
    }
}
