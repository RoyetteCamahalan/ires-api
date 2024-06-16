using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateRentalChargeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rentalcharges",
                columns: table => new
                {
                    chargeid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contractid = table.Column<long>(type: "bigint", nullable: false),
                    otherfeeid = table.Column<long>(type: "bigint", nullable: true),
                    chargedate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    chargeamount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    interestamount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    chargetype = table.Column<int>(type: "int", nullable: false),
                    runningbalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    interestype = table.Column<int>(type: "int", nullable: false),
                    createdbyid = table.Column<long>(type: "bigint", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedbyid = table.Column<long>(type: "bigint", nullable: true),
                    dateupdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rentalcharges", x => x.chargeid);
                    table.ForeignKey(
                        name: "FK_rentalcharges_otherfees_otherfeeid",
                        column: x => x.otherfeeid,
                        principalTable: "otherfees",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_rentalcharges_otherfeeid",
                table: "rentalcharges",
                column: "otherfeeid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "rentalcharges");
        }
    }
}
