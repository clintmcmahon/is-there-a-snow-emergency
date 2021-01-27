using Microsoft.EntityFrameworkCore.Migrations;

namespace SnowEmergency.Models.Migrations
{
    public partial class updatehtmltobody : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HTML",
                table: "Notices",
                newName: "Body");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Body",
                table: "Notices",
                newName: "HTML");
        }
    }
}
