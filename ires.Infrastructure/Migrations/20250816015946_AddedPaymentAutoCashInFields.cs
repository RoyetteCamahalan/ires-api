using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedPaymentAutoCashInFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "refpaymentid",
                table: "pettycashdisbursement",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "autocashinaccountid",
                table: "payment",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "refpaymentid",
                table: "pettycashdisbursement");

            migrationBuilder.DropColumn(
                name: "autocashinaccountid",
                table: "payment");
        }
    }
}
