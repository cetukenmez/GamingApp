using Microsoft.EntityFrameworkCore.Migrations;

namespace GamingApp.Migrations
{
    public partial class logupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Logs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Logs_GameId",
                table: "Logs",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Game_GameId",
                table: "Logs",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Game_GameId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_GameId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Logs");
        }
    }
}
