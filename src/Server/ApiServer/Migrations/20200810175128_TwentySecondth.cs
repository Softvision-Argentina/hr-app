namespace ApiServer.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class TwentySecondth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "Roles",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "isReviewer",
                table: "Employees",
                newName: "IsReviewer");

            migrationBuilder.CreateTable(
                name: "ProfilesByCommunity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Version = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    CommunityId = table.Column<int>(nullable: true),
                    ProfileId = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilesByCommunity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfilesByCommunity_Community_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Community",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfilesByCommunity_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SkillProfile",
                columns: table => new
                {
                    SkillId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillProfile", x => new { x.SkillId, x.ProfileId });
                    table.ForeignKey(
                        name: "FK_SkillProfile_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SkillProfile_SkillTypes_SkillId",
                        column: x => x.SkillId,
                        principalTable: "SkillTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfilesByCommunity_CommunityId",
                table: "ProfilesByCommunity",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfilesByCommunity_ProfileId",
                table: "ProfilesByCommunity",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillProfile_ProfileId",
                table: "SkillProfile",
                column: "ProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfilesByCommunity");

            migrationBuilder.DropTable(
                name: "SkillProfile");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Roles",
                newName: "isActive");

            migrationBuilder.RenameColumn(
                name: "IsReviewer",
                table: "Employees",
                newName: "isReviewer");
        }
    }
}
