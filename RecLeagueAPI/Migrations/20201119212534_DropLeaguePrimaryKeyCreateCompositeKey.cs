using Microsoft.EntityFrameworkCore.Migrations;

//GAVE THIS THE WRONG NAME
//INSTEAD THIS MIGRATION MAKES AN INDEX OF SportId, RegionId, AND Tier AND GIVES THE INDEX A UNIQUE CONSTRAINT

namespace RecLeagueAPI.Migrations
{
    public partial class DropLeaguePrimaryKeyCreateCompositeKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_League_SportId",
                table: "League");

            migrationBuilder.CreateIndex(
                name: "IX_League_SportId_RegionId_Tier",
                table: "League",
                columns: new[] { "SportId", "RegionId", "Tier" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_League_SportId_RegionId_Tier",
                table: "League");

            migrationBuilder.CreateIndex(
                name: "IX_League_SportId",
                table: "League",
                column: "SportId");
        }
    }
}
