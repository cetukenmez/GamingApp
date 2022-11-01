using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GamingApp.Migrations
{
    public partial class sessiondate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SessionTime",
                table: "Session",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionTime",
                table: "Session");
        }
    }
}
