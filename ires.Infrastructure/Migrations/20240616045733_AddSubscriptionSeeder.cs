using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "subscriptionplan",
                columns: new[] { "id", "description", "isactive", "moduleid", "monthlysubscription", "name", "storage", "surveylimit" },
                values: new object[,]
                {
                    { 1, "", true, 15, 0m, "60 Day Trial", 1000m, 0 },
                    { 2, "", true, 15, 800m, "Surveying Pro", 1000m, 0 },
                    { 3, "", true, 15, 1000m, "Surveying Enterprise", 5000m, 0 },
                    { 4, "", true, 10, 0m, "60 Day Trial", 1000m, 0 },
                    { 5, "", true, 10, 500m, "Finance Enterprise", 5000m, 0 },
                    { 6, "", true, 14, 0m, "60 Day Trial", 1000m, 0 },
                    { 7, "", true, 14, 800m, "Rental Pro", 1000m, 20 },
                    { 8, "", true, 14, 1000m, "Rental Enterprise", 5000m, 0 },
                    { 9, "", true, 14, 800m, "Rental Pro + Surveying Pro", 1000m, 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "subscriptionplan",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "subscriptionplan",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "subscriptionplan",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "subscriptionplan",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "subscriptionplan",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "subscriptionplan",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "subscriptionplan",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "subscriptionplan",
                keyColumn: "id",
                keyValue: 8);
        }
    }
}
