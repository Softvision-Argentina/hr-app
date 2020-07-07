using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiServer.Migrations
{
    public partial class Nineteenth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OpenPositionId",
                table: "Candidates",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PositionTitle",
                table: "Candidates",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_OpenPositionId",
                table: "Candidates",
                column: "OpenPositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_OpenPositions_OpenPositionId",
                table: "Candidates",
                column: "OpenPositionId",
                principalTable: "OpenPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_OpenPositions_OpenPositionId",
                table: "Candidates");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_OpenPositionId",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "OpenPositionId",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "PositionTitle",
                table: "Candidates");
        }
    }
}
