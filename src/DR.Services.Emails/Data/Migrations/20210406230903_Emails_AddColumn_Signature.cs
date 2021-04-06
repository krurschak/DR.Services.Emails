using Microsoft.EntityFrameworkCore.Migrations;

namespace DR.Services.Emails.Data.Migrations
{
    public partial class Emails_AddColumn_Signature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Signature",
                table: "Emails",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Signature",
                table: "Emails");
        }
    }
}
