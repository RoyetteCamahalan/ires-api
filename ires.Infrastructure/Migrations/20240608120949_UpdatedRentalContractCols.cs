using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedRentalContractCols : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_rentalcontracts_propertyrentals_propertyid",
                table: "rentalcontracts");

            migrationBuilder.DropIndex(
                name: "IX_rentalcontracts_propertyid",
                table: "rentalcontracts");

            migrationBuilder.DropColumn(
                name: "propertyid",
                table: "rentalcontracts");

            migrationBuilder.AddColumn<decimal>(
                name: "ewtpercentage",
                table: "rentalcontracts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "monthlypenalty",
                table: "rentalcontracts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "penaltyextension",
                table: "rentalcontracts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ewtpercentage",
                table: "rentalcontracts");

            migrationBuilder.DropColumn(
                name: "monthlypenalty",
                table: "rentalcontracts");

            migrationBuilder.DropColumn(
                name: "penaltyextension",
                table: "rentalcontracts");

            migrationBuilder.AddColumn<long>(
                name: "propertyid",
                table: "rentalcontracts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_rentalcontracts_propertyid",
                table: "rentalcontracts",
                column: "propertyid");

            migrationBuilder.AddForeignKey(
                name: "FK_rentalcontracts_propertyrentals_propertyid",
                table: "rentalcontracts",
                column: "propertyid",
                principalTable: "propertyrentals",
                principalColumn: "propertyid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
