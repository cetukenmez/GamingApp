using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GamingApp.Migrations
{
    public partial class oyunturu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OyunTuruId",
                table: "Game",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OyunTuru",
                columns: table => new
                {
                    OyunTuruId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OyunTuruAdi = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OyunTuru", x => x.OyunTuruId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Game_OyunTuruId",
                table: "Game",
                column: "OyunTuruId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_OyunTuru_OyunTuruId",
                table: "Game",
                column: "OyunTuruId",
                principalTable: "OyunTuru",
                principalColumn: "OyunTuruId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_OyunTuru_OyunTuruId",
                table: "Game");

            migrationBuilder.DropTable(
                name: "OyunTuru");

            migrationBuilder.DropIndex(
                name: "IX_Game_OyunTuruId",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "OyunTuruId",
                table: "Game");
        }
    }
}
