using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateCompanySettingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "companysettings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    companyid = table.Column<int>(type: "int", nullable: false),
                    autocashinaccountid_survey = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companysettings", x => x.id);
                    table.ForeignKey(
                        name: "FK_companysettings_accounts_autocashinaccountid_survey",
                        column: x => x.autocashinaccountid_survey,
                        principalTable: "accounts",
                        principalColumn: "accountid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_companysettings_autocashinaccountid_survey",
                table: "companysettings",
                column: "autocashinaccountid_survey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "companysettings");
        }
    }
}
