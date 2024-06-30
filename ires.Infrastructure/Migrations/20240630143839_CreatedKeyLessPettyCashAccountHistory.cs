using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreatedKeyLessPettyCashAccountHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pettyCashAccountHistories",
                columns: table => new
                {
                    transid = table.Column<long>(type: "bigint", nullable: false),
                    transtype = table.Column<int>(type: "int", nullable: false),
                    refno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    particular = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    transdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    actualdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    runningbalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pettyCashAccountHistories");
        }
    }
}
