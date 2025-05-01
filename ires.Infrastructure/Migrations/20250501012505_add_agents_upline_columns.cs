using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_agents_upline_columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "upline_id",
                table: "agents",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_agents_upline_id",
                table: "agents",
                column: "upline_id");

            migrationBuilder.AddForeignKey(
                name: "FK_agents_agents_upline_id",
                table: "agents",
                column: "upline_id",
                principalTable: "agents",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_agents_agents_upline_id",
                table: "agents");

            migrationBuilder.DropIndex(
                name: "IX_agents_upline_id",
                table: "agents");

            migrationBuilder.DropColumn(
                name: "upline_id",
                table: "agents");
        }
    }
}
