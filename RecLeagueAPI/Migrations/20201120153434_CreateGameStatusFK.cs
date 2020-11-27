using Microsoft.EntityFrameworkCore.Migrations;

namespace RecLeagueAPI.Migrations
{
    public partial class CreateGameStatusFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Player_GameStatusId",
                table: "Player",
                column: "GameStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_GameStatus_GameStatusId",
                table: "Player",
                column: "GameStatusId",
                principalTable: "GameStatus",
                principalColumn: "GameStatusId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_GameStatus_GameStatusId",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_GameStatusId",
                table: "Player");
        }
    }
}
