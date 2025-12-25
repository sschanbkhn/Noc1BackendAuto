using Microsoft.EntityFrameworkCore.Migrations;

namespace Network.API.Infrastructure.migrations
{
    public partial class InitDB3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Net_UC_LinhVuc",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Net_UC_LinhVuc");
        }
    }
}
