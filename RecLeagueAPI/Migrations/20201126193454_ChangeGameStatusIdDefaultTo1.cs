using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RecLeagueAPI.Migrations
{
    public partial class ChangeGameStatusIdDefaultTo1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GameStatusId",
                table: "Player",
                type: "int",
                nullable: false,
                defaultValue: 1);
                
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GameStatusId",
                table: "Player",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
