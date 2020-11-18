using Microsoft.EntityFrameworkCore.Migrations;

namespace RecLeagueAPI.Migrations
{
    public partial class AddPlayerColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "staySignedIn",
                table: "Player",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "staySignedIn",
                table: "Player");
        }
    }
}
