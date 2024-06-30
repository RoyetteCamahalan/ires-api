using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateExpenseTypeCategorySeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "expensetypecategory",
                columns: new[] { "expensecatid", "description", "isactive" },
                values: new object[,]
                {
                    { 1, "Operating Expense", true },
                    { 2, "Non-Operating Expense", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "expensetypecategory",
                keyColumn: "expensecatid",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "expensetypecategory",
                keyColumn: "expensecatid",
                keyValue: 2);
        }
    }
}
