using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiServer.Migrations
{
    public partial class Eighth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PreOfferStages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ProcessId = table.Column<int>(nullable: false),
                    UserOwnerId = table.Column<int>(nullable: true),
                    UserDelegateId = table.Column<int>(nullable: true),
                    RejectionReason = table.Column<string>(nullable: true),
                    DNI = table.Column<int>(nullable: false),
                    BackgroundCheckDone = table.Column<bool>(nullable: false),
                    BackgroundCheckDoneDate = table.Column<DateTime>(nullable: true),
                    BornDate = table.Column<DateTime>(nullable: true),
                    PreocupationalDone = table.Column<bool>(nullable: false),
                    PreocupationalDoneDate = table.Column<DateTime>(nullable: true),
                    RemunerationOffer = table.Column<int>(nullable: false),
                    VacationDays = table.Column<int>(nullable: false),
                    Firstday = table.Column<DateTime>(nullable: false),
                    Bonus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreOfferStages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreOfferStages_Processes_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "Processes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreOfferStages_Users_UserDelegateId",
                        column: x => x.UserDelegateId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreOfferStages_Users_UserOwnerId",
                        column: x => x.UserOwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreOfferStages_ProcessId",
                table: "PreOfferStages",
                column: "ProcessId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreOfferStages_UserDelegateId",
                table: "PreOfferStages",
                column: "UserDelegateId");

            migrationBuilder.CreateIndex(
                name: "IX_PreOfferStages_UserOwnerId",
                table: "PreOfferStages",
                column: "UserOwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreOfferStages");
        }
    }
}
