using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedAppPreferencewithSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "apppreferences",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_apppreferences", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "apppreferences",
                columns: new[] { "id", "name", "value" },
                values: new object[,]
                {
                    { 1, "CRON Reload Subsrciption", "1990/01/01" },
                    { 2, "CRON Reload Rentals", "1990/01/01" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "apppreferences");
        }
    }
}
