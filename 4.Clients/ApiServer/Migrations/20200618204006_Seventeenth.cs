using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiServer.Migrations
{
    public partial class Seventeenth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OpenPositions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Seniority = table.Column<int>(nullable: false),
                    Studio = table.Column<string>(nullable: true),
                    CommunityId = table.Column<int>(nullable: true),
                    Priority = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenPositions_Community_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Community",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OpenPositions_CommunityId",
                table: "OpenPositions",
                column: "CommunityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpenPositions");
        }
    }
}
