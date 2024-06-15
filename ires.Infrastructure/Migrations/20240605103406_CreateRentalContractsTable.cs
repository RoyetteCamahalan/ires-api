using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateRentalContractsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rentalcontracts",
                columns: table => new
                {
                    contractid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contractno = table.Column<long>(type: "bigint", nullable: false),
                    custid = table.Column<long>(type: "bigint", nullable: false),
                    propertyid = table.Column<long>(type: "bigint", nullable: false),
                    contractdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    montlyrent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    deposit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    term = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    totalbalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    noofmonthdeposit = table.Column<int>(type: "int", nullable: false),
                    noofmonthadvance = table.Column<int>(type: "int", nullable: false),
                    advancerent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    billingstart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    createdbyid = table.Column<long>(type: "bigint", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedbyid = table.Column<long>(type: "bigint", nullable: false),
                    dateupdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rentalcontracts", x => x.contractid);
                    table.ForeignKey(
                        name: "FK_rentalcontracts_customer_custid",
                        column: x => x.custid,
                        principalTable: "customer",
                        principalColumn: "custid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rentalcontracts_propertyrentals_propertyid",
                        column: x => x.propertyid,
                        principalTable: "propertyrentals",
                        principalColumn: "propertyid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_rentalcontracts_custid",
                table: "rentalcontracts",
                column: "custid");

            migrationBuilder.CreateIndex(
                name: "IX_rentalcontracts_propertyid",
                table: "rentalcontracts",
                column: "propertyid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rentalcontracts");
        }
    }
}
