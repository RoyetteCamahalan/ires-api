using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmployeeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "dateupdated",
                table: "employees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "updatedbyid",
                table: "employees",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dateupdated",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "updatedbyid",
                table: "employees");
        }
    }
}
