using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    clientid = table.Column<long>(type: "bigint", nullable: false),
                    carid = table.Column<long>(type: "bigint", nullable: false),
                    startdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    enddate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    noofdays = table.Column<int>(type: "int", nullable: false),
                    ratetype = table.Column<int>(type: "int", nullable: false),
                    rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    totalrate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    drivername = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    isselfdrive = table.Column<bool>(type: "bit", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    remarks = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookings", x => x.id);
                    table.ForeignKey(
                        name: "FK_bookings_cars_carid",
                        column: x => x.carid,
                        principalTable: "cars",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bookings_customer_clientid",
                        column: x => x.clientid,
                        principalTable: "customer",
                        principalColumn: "custid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bookings_carid",
                table: "bookings",
                column: "carid");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_clientid",
                table: "bookings",
                column: "clientid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookings");
        }
    }
}
