using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BillingAccount_NotifyOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotifyAllUsers",
                table: "billing_accounts");

            migrationBuilder.AddColumn<string>(
                name: "NotifyOption",
                table: "billing_accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotifyOption",
                table: "billing_accounts");

            migrationBuilder.AddColumn<bool>(
                name: "NotifyAllUsers",
                table: "billing_accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
