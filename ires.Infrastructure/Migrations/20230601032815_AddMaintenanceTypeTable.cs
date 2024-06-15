using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMaintenanceTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "maintenancetypes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    isactive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_maintenancetypes", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "maintenancetypes",
                columns: new[] { "id", "isactive", "name" },
                values: new object[,]
                {
                    { 1, true, "Repair and Maintenance" },
                    { 2, true, "Registration Renewal" },
                    { 3, true, "Personal Use" },
                    { 4, true, "Others" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "maintenancetypes");
        }
    }
}
