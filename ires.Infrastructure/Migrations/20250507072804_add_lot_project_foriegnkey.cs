using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_lot_project_foriegnkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_lot_propertyid",
                table: "lot",
                column: "propertyid");

            migrationBuilder.AddForeignKey(
                name: "FK_lot_property_propertyid",
                table: "lot",
                column: "propertyid",
                principalTable: "property",
                principalColumn: "propertyid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lot_property_propertyid",
                table: "lot");

            migrationBuilder.DropIndex(
                name: "IX_lot_propertyid",
                table: "lot");
        }
    }
}
