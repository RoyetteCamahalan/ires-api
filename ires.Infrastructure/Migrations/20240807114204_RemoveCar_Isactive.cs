using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCar_Isactive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isactive",
                table: "cars");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isactive",
                table: "cars",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
