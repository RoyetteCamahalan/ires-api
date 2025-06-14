using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class create_commissiondetails_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "commissiondetails",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contractid = table.Column<long>(type: "bigint", nullable: false),
                    agentid = table.Column<long>(type: "bigint", nullable: false),
                    compercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    referralpercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isagent = table.Column<bool>(type: "bit", nullable: false),
                    createdbyid = table.Column<long>(type: "bigint", nullable: false),
                    datecreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedbyid = table.Column<long>(type: "bigint", nullable: true),
                    dateupdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commissiondetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_commissiondetails_agents_agentid",
                        column: x => x.agentid,
                        principalTable: "agents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_commissiondetails_agentid",
                table: "commissiondetails",
                column: "agentid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "commissiondetails");
        }
    }
}
