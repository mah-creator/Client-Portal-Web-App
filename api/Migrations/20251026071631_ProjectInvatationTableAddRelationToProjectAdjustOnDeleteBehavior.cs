using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientPortalApi.Migrations
{
    /// <inheritdoc />
    public partial class ProjectInvatationTableAddRelationToProjectAdjustOnDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Users_InviteeId",
                table: "Invitations");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_ProjectId",
                table: "Invitations",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Projects_ProjectId",
                table: "Invitations",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Users_InviteeId",
                table: "Invitations",
                column: "InviteeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Projects_ProjectId",
                table: "Invitations");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_Users_InviteeId",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_ProjectId",
                table: "Invitations");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_Users_InviteeId",
                table: "Invitations",
                column: "InviteeId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
