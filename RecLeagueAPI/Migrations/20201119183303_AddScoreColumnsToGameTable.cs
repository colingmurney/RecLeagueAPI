using Microsoft.EntityFrameworkCore.Migrations;

namespace RecLeagueAPI.Migrations
{
    public partial class AddScoreColumnsToGameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AwayTeamAwayScore",
                table: "Game",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamHomeScore",
                table: "Game",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamAwayScore",
                table: "Game",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamHomeScore",
                table: "Game",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayTeamAwayScore",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "AwayTeamHomeScore",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "HomeTeamAwayScore",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "HomeTeamHomeScore",
                table: "Game");
        }
    }
}
