using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class create_contract_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "contracts",
                columns: table => new
                {
                    contractid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    contractno = table.Column<long>(type: "bigint", nullable: false),
                    custid = table.Column<long>(type: "bigint", nullable: false),
                    contractdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    term = table.Column<int>(type: "int", nullable: false),
                    billingsched = table.Column<int>(type: "int", nullable: false),
                    monthlypenalty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    penaltyextension = table.Column<int>(type: "int", nullable: false),
                    commissionterm = table.Column<int>(type: "int", nullable: false),
                    remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdbyid = table.Column<long>(type: "bigint", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedbyid = table.Column<long>(type: "bigint", nullable: true),
                    dateupdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contracts", x => x.contractid);
                    table.ForeignKey(
                        name: "FK_contracts_customer_custid",
                        column: x => x.custid,
                        principalTable: "customer",
                        principalColumn: "custid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_contracts_custid",
                table: "contracts",
                column: "custid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contracts");
        }
    }
}
