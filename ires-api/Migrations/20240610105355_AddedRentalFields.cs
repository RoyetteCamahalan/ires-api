using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires_api.Migrations
{
    /// <inheritdoc />
    public partial class AddedRentalFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "companyid",
                table: "rentalcontracts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "remarks",
                table: "rentalcontracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "companyid",
                table: "rentalcontracts");

            migrationBuilder.DropColumn(
                name: "remarks",
                table: "rentalcontracts");
        }
    }
}
