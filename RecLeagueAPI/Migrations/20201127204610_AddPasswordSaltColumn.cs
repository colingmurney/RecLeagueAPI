using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RecLeagueAPI.Migrations
{
    public partial class AddPasswordSaltColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Player",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {              

        }
    }
}
