using Microsoft.EntityFrameworkCore.Migrations;

namespace RecLeagueAPI.Migrations
{
    public partial class ChangePlayerFieldsToBoolean : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "staySignedIn",
                table: "Player",
                newName: "StaySignedIn");

            migrationBuilder.AlterColumn<bool>(
                name: "StaySignedIn",
                table: "Player",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<byte>(
                name: "IsCaptain",
                table: "Player",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StaySignedIn",
                table: "Player",
                newName: "staySignedIn");

            migrationBuilder.AlterColumn<byte>(
                name: "staySignedIn",
                table: "Player",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<byte>(
                name: "IsCaptain",
                table: "Player",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");
        }
    }
}
