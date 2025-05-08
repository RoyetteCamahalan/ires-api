using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class create_lotmodel_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "lot");

            migrationBuilder.AddColumn<long>(
                name: "model_id",
                table: "lot",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "lot_models",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    project_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdbyid = table.Column<long>(type: "bigint", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedbyid = table.Column<long>(type: "bigint", nullable: true),
                    dateupdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lot_models", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_lot_lot_models_propertyid",
                table: "lot",
                column: "propertyid",
                principalTable: "lot_models",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lot_lot_models_propertyid",
                table: "lot");

            migrationBuilder.DropTable(
                name: "lot_models");

            migrationBuilder.DropColumn(
                name: "model_id",
                table: "lot");

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "lot",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
