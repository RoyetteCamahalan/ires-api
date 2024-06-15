using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProjectAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "address",
                table: "propertyrentals");

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "propertyrentals",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "propertyrentals");

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "propertyrentals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
