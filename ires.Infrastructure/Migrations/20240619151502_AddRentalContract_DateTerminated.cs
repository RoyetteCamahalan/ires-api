using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRentalContract_DateTerminated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Payable",
                newName: "payables");

            migrationBuilder.AddColumn<DateTime>(
                name: "dateterminated",
                table: "rentalcontracts",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dateterminated",
                table: "rentalcontracts");

            migrationBuilder.RenameTable(
                name: "payables",
                newName: "Payable");
        }
    }
}
