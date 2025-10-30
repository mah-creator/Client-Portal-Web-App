using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientPortalApi.Migrations
{
    /// <inheritdoc />
    public partial class StripeProjectCheckoutSessionProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Paid",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StripeCheckoutSessionId",
                table: "Projects",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Paid",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "StripeCheckoutSessionId",
                table: "Projects");
        }
    }
}
