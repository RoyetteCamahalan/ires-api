using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceRentaContractBillingStart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "billingstart",
                table: "rentalcontracts");

            migrationBuilder.AddColumn<int>(
                name: "billingsched",
                table: "rentalcontracts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "billingsched",
                table: "rentalcontracts");

            migrationBuilder.AddColumn<DateTime>(
                name: "billingstart",
                table: "rentalcontracts",
                type: "datetime2",
                nullable: true);
        }
    }
}
