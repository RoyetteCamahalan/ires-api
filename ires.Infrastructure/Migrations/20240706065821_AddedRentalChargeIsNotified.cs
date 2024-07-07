using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedRentalChargeIsNotified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isnotified",
                table: "rentalcharges",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isnotified",
                table: "rentalcharges");
        }
    }
}
