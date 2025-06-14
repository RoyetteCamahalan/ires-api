using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_status_in_lot_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isactive",
                table: "lot");

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "lot",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "lot");

            migrationBuilder.AddColumn<bool>(
                name: "isactive",
                table: "lot",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
