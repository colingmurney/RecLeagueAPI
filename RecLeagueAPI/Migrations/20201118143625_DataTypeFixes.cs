using Microsoft.EntityFrameworkCore.Migrations;

namespace RecLeagueAPI.Migrations
{
    public partial class DataTypeFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "tier",
                table: "League",
                newName: "Tier");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Venue",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsCaptain",
                table: "Player",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<bool>(
                name: "GameStatus",
                table: "Player",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tier",
                table: "League",
                newName: "tier");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Venue",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<byte>(
                name: "IsCaptain",
                table: "Player",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<byte>(
                name: "GameStatus",
                table: "Player",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
