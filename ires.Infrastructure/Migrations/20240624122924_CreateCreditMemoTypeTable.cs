using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateCreditMemoTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "creditmemotypeid",
                table: "payment",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "creditmemotypes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isactive = table.Column<bool>(type: "bit", nullable: false),
                    createdbyid = table.Column<long>(type: "bigint", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedbyid = table.Column<long>(type: "bigint", nullable: true),
                    dateupdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_creditmemotypes", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_payment_creditmemotypeid",
                table: "payment",
                column: "creditmemotypeid");

            migrationBuilder.AddForeignKey(
                name: "FK_payment_creditmemotypes_creditmemotypeid",
                table: "payment",
                column: "creditmemotypeid",
                principalTable: "creditmemotypes",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payment_creditmemotypes_creditmemotypeid",
                table: "payment");

            migrationBuilder.DropTable(
                name: "creditmemotypes");

            migrationBuilder.DropIndex(
                name: "IX_payment_creditmemotypeid",
                table: "payment");

            migrationBuilder.DropColumn(
                name: "creditmemotypeid",
                table: "payment");
        }
    }
}
