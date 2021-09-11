using Microsoft.EntityFrameworkCore.Migrations;

namespace MySkillsServer.Data.Migrations
{
    public partial class AddIpToContactFormMessageEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ip",
                table: "ContactFormMessages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ip",
                table: "ContactFormMessages");
        }
    }
}
