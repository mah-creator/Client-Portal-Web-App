using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientPortalApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTaskFileRelation2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_TaskItems_TaskId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_TaskId",
                table: "Files");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Files_TaskId",
                table: "Files",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_TaskItems_TaskId",
                table: "Files",
                column: "TaskId",
                principalTable: "TaskItems",
                principalColumn: "Id");
        }
    }
}
