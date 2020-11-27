using Microsoft.EntityFrameworkCore.Migrations;

namespace RecLeagueAPI.Migrations
{
    public partial class CreateGameStatusTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameStatus",
                table: "Player");

            migrationBuilder.AddColumn<int>(
                name: "GameStatusId",
                table: "Player",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GameStatus",
                columns: table => new
                {
                    GameStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameStatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStatus", x => x.GameStatusId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameStatus");

            migrationBuilder.DropColumn(
                name: "GameStatusId",
                table: "Player");

            migrationBuilder.AddColumn<byte>(
                name: "GameStatus",
                table: "Player",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
