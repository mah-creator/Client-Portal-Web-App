using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientPortalApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeImageToAvatar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Profile",
                newName: "AvatarPath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvatarPath",
                table: "Profile",
                newName: "ImageUrl");
        }
    }
}
