using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable


namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCarTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cartypes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isactive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cartypes", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "cartypes",
                columns: new[] { "id", "isactive", "name" },
                values: new object[,]
                {
                    { 1, true, "Hatchback" },
                    { 2, true, "Sedan" },
                    { 3, true, "Minivan" },
                    { 4, true, "Crossover" },
                    { 5, true, "Pickup" },
                    { 6, true, "SUV" },
                    { 7, true, "Van" },
                    { 8, true, "Others" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cartypes");
        }
    }
}
