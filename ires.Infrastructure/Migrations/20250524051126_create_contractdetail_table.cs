using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class create_contractdetail_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "contractdetails",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    contractid = table.Column<long>(type: "bigint", nullable: false),
                    lotid = table.Column<long>(type: "bigint", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    commissionableprice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    commissionpercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    amortization = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    adcom = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    referralfee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    dateforfeited = table.Column<DateTime>(type: "datetime2", nullable: true),
                    forfeitreason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    forfeitedbyid = table.Column<long>(type: "bigint", nullable: true),
                    hasrealtytax = table.Column<bool>(type: "bit", nullable: false),
                    totalcapitalgains = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    totaltaxes = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    totalotherfees = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    totallotbalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    totaldownpayment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    istitlereleased = table.Column<bool>(type: "bit", nullable: false),
                    titlereleasedate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    titlereleasedby = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    titlereceivedby = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    totalarrears = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    createdbyid = table.Column<long>(type: "bigint", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedbyid = table.Column<long>(type: "bigint", nullable: true),
                    dateupdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contractdetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_contractdetails_contracts_contractid",
                        column: x => x.contractid,
                        principalTable: "contracts",
                        principalColumn: "contractid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_contractdetails_lot_lotid",
                        column: x => x.lotid,
                        principalTable: "lot",
                        principalColumn: "lot_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_contractdetails_contractid",
                table: "contractdetails",
                column: "contractid");

            migrationBuilder.CreateIndex(
                name: "IX_contractdetails_lotid",
                table: "contractdetails",
                column: "lotid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contractdetails");
        }
    }
}
