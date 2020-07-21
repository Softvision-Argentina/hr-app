using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiServer.Migrations
{
    public partial class Fourth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalInformation",
                table: "Candidates");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInformation",
                table: "HrStages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalInformation",
                table: "HrStages");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInformation",
                table: "Candidates",
                nullable: true);
        }
    }
}
