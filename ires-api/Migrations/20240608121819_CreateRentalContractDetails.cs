using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires_api.Migrations
{
    /// <inheritdoc />
    public partial class CreateRentalContractDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rentalcontractdetails",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contractid = table.Column<long>(type: "bigint", nullable: false),
                    propertyid = table.Column<long>(type: "bigint", nullable: false),
                    createdbyid = table.Column<long>(type: "bigint", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rentalcontractdetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_rentalcontractdetails_propertyrentals_propertyid",
                        column: x => x.propertyid,
                        principalTable: "propertyrentals",
                        principalColumn: "propertyid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rentalcontractdetails_rentalcontracts_contractid",
                        column: x => x.contractid,
                        principalTable: "rentalcontracts",
                        principalColumn: "contractid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_rentalcontractdetails_contractid",
                table: "rentalcontractdetails",
                column: "contractid");

            migrationBuilder.CreateIndex(
                name: "IX_rentalcontractdetails_propertyid",
                table: "rentalcontractdetails",
                column: "propertyid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rentalcontractdetails");
        }
    }
}
