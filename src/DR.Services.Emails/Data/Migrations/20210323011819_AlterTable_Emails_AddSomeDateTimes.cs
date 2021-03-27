using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DR.Services.Emails.Data.Migrations
{
    public partial class AlterTable_Emails_AddSomeDateTimes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDateUtc",
                table: "Emails",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateUtc",
                table: "Emails",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SentDateUtc",
                table: "Emails",
                type: "timestamp without time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDateUtc",
                table: "Emails");

            migrationBuilder.DropColumn(
                name: "LastUpdateUtc",
                table: "Emails");

            migrationBuilder.DropColumn(
                name: "SentDateUtc",
                table: "Emails");
        }
    }
}
