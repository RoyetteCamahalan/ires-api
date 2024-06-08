using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires_api.Migrations
{
    /// <inheritdoc />
    public partial class CreateRentalProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "projectypeid",
                table: "property",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "projectypeid",
                table: "property");
        }
    }
}
