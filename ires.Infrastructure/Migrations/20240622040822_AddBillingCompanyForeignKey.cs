using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBillingCompanyForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_bill_companyid",
                table: "bill",
                column: "companyid");

            migrationBuilder.AddForeignKey(
                name: "FK_bill_company_companyid",
                table: "bill",
                column: "companyid",
                principalTable: "company",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bill_company_companyid",
                table: "bill");

            migrationBuilder.DropIndex(
                name: "IX_bill_companyid",
                table: "bill");
        }
    }
}
