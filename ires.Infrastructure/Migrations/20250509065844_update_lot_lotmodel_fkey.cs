using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_lot_lotmodel_fkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lot_lot_models_propertyid",
                table: "lot");

            migrationBuilder.CreateIndex(
                name: "IX_lot_model_id",
                table: "lot",
                column: "model_id");

            migrationBuilder.AddForeignKey(
                name: "FK_lot_lot_models_model_id",
                table: "lot",
                column: "model_id",
                principalTable: "lot_models",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lot_lot_models_model_id",
                table: "lot");

            migrationBuilder.DropIndex(
                name: "IX_lot_model_id",
                table: "lot");

            migrationBuilder.AddForeignKey(
                name: "FK_lot_lot_models_propertyid",
                table: "lot",
                column: "propertyid",
                principalTable: "lot_models",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
