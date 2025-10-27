using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientPortalApi.Migrations
{
    /// <inheritdoc />
    public partial class ProjectInvatationTableAdjustOnDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Users_InviteeId",
                table: "Invitations");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Users_InviteeId",
                table: "Invitations",
                column: "InviteeId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Users_InviteeId",
                table: "Invitations");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Users_InviteeId",
                table: "Invitations",
                column: "InviteeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
