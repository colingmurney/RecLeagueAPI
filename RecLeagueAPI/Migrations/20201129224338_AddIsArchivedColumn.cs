using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RecLeagueAPI.Migrations
{
    public partial class AddIsArchivedColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Game",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Game");      
        }
    }
}
