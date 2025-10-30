using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientPortalApi.Migrations
{
    /// <inheritdoc />
    public partial class StripeSpecificColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StripeCustomerId",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StripePriceId",
                table: "Projects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeProductId",
                table: "Projects",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeCustomerId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "StripePriceId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "StripeProductId",
                table: "Projects");
        }
    }
}
