using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRentalChargeForeignKeyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_rentalcharges_contractid",
                table: "rentalcharges",
                column: "contractid");

            migrationBuilder.AddForeignKey(
                name: "FK_rentalcharges_rentalcontracts_contractid",
                table: "rentalcharges",
                column: "contractid",
                principalTable: "rentalcontracts",
                principalColumn: "contractid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rentalcharges_rentalcontracts_contractid",
                table: "rentalcharges");

            migrationBuilder.DropIndex(
                name: "IX_rentalcharges_contractid",
                table: "rentalcharges");
        }
    }
}
