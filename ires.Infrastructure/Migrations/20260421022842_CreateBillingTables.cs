using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateBillingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "billing_accounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VendorId = table.Column<long>(type: "bigint", nullable: false),
                    OfficeId = table.Column<long>(type: "bigint", nullable: false),
                    ExpenseTypeId = table.Column<long>(type: "bigint", nullable: false),
                    HasFixedAmount = table.Column<bool>(type: "bit", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Frequency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueDayOfMonth = table.Column<int>(type: "int", nullable: true),
                    DueDayOfWeek = table.Column<int>(type: "int", nullable: true),
                    NotifyDaysBefore = table.Column<int>(type: "int", nullable: false),
                    NotifyAllUsers = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Memo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_billing_accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_billing_accounts_accounts_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "accounts",
                        principalColumn: "accountid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_billing_accounts_expensetypes_ExpenseTypeId",
                        column: x => x.ExpenseTypeId,
                        principalTable: "expensetypes",
                        principalColumn: "expensetypeid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_billing_accounts_vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "vendors",
                        principalColumn: "vendorid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "billing_payments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    BillingAccountId = table.Column<long>(type: "bigint", nullable: false),
                    ExpenseId = table.Column<long>(type: "bigint", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PeriodFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PeriodTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedById = table.Column<long>(type: "bigint", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_billing_payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_billing_payments_billing_accounts_BillingAccountId",
                        column: x => x.BillingAccountId,
                        principalTable: "billing_accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_billing_payments_expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "expenses",
                        principalColumn: "expenseid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_billing_accounts_ExpenseTypeId",
                table: "billing_accounts",
                column: "ExpenseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_billing_accounts_OfficeId",
                table: "billing_accounts",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_billing_accounts_VendorId",
                table: "billing_accounts",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_BillingAccount_CompanyId",
                table: "billing_accounts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_billing_payments_BillingAccountId",
                table: "billing_payments",
                column: "BillingAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_billing_payments_ExpenseId",
                table: "billing_payments",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_BillingPayment_CompanyId",
                table: "billing_payments",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "billing_payments");

            migrationBuilder.DropTable(
                name: "billing_accounts");
        }
    }
}
