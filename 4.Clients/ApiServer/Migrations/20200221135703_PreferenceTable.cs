using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiServer.Migrations
{
    public partial class PreferenceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Preferences",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    CasualtiesDashboard = table.Column<bool>(nullable: false),
                    CompletedDashboard = table.Column<bool>(nullable: false),
                    ProcessesDashboard = table.Column<bool>(nullable: false),
                    ProgressDashboard = table.Column<bool>(nullable: false),
                    ProjectionDashboard = table.Column<bool>(nullable: false),
                    SkillsDashboard = table.Column<bool>(nullable: false),
                    TimeToFill1Dashboard = table.Column<bool>(nullable: false),
                    TimeToFIll2Dashboard = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Preferences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_UserId",
                table: "Preferences",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Preferences");
        }
    }
}
