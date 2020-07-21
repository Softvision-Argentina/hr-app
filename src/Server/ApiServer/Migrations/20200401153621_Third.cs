using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiServer.Migrations
{
    public partial class Third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_Consultants_RecruiterId",
                table: "Candidates");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientStages_Consultants_ConsultantDelegateId",
                table: "ClientStages");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientStages_Consultants_ConsultantOwnerId",
                table: "ClientStages");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Consultants_RecruiterId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_HrStages_Consultants_ConsultantDelegateId",
                table: "HrStages");

            migrationBuilder.DropForeignKey(
                name: "FK_HrStages_Consultants_ConsultantOwnerId",
                table: "HrStages");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferStages_Consultants_ConsultantDelegateId",
                table: "OfferStages");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferStages_Consultants_ConsultantOwnerId",
                table: "OfferStages");

            migrationBuilder.DropForeignKey(
                name: "FK_Processes_Consultants_ConsultantDelegateId",
                table: "Processes");

            migrationBuilder.DropForeignKey(
                name: "FK_Processes_Consultants_ConsultantOwnerId",
                table: "Processes");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Consultants_RecruiterId",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Stages_Consultants_ConsultantDelegateId",
                table: "Stages");

            migrationBuilder.DropForeignKey(
                name: "FK_Stages_Consultants_ConsultantOwnerId",
                table: "Stages");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Consultants_ConsultantId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalStages_Consultants_ConsultantDelegateId",
                table: "TechnicalStages");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalStages_Consultants_ConsultantOwnerId",
                table: "TechnicalStages");

            migrationBuilder.DropTable(
                name: "Consultants");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_RecruiterId",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "RecruiterId",
                table: "Candidates");

            migrationBuilder.RenameColumn(
                name: "ConsultantOwnerId",
                table: "TechnicalStages",
                newName: "UserOwnerId");

            migrationBuilder.RenameColumn(
                name: "ConsultantDelegateId",
                table: "TechnicalStages",
                newName: "UserDelegateId");

            migrationBuilder.RenameIndex(
                name: "IX_TechnicalStages_ConsultantOwnerId",
                table: "TechnicalStages",
                newName: "IX_TechnicalStages_UserOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_TechnicalStages_ConsultantDelegateId",
                table: "TechnicalStages",
                newName: "IX_TechnicalStages_UserDelegateId");

            migrationBuilder.RenameColumn(
                name: "ConsultantId",
                table: "Tasks",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_ConsultantId",
                table: "Tasks",
                newName: "IX_Tasks_UserId");

            migrationBuilder.RenameColumn(
                name: "ConsultantOwnerId",
                table: "Stages",
                newName: "UserOwnerId");

            migrationBuilder.RenameColumn(
                name: "ConsultantDelegateId",
                table: "Stages",
                newName: "UserDelegateId");

            migrationBuilder.RenameIndex(
                name: "IX_Stages_ConsultantOwnerId",
                table: "Stages",
                newName: "IX_Stages_UserOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Stages_ConsultantDelegateId",
                table: "Stages",
                newName: "IX_Stages_UserDelegateId");

            migrationBuilder.RenameColumn(
                name: "RecruiterId",
                table: "Reservation",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_RecruiterId",
                table: "Reservation",
                newName: "IX_Reservation_UserId");

            migrationBuilder.RenameColumn(
                name: "ConsultantOwnerId",
                table: "Processes",
                newName: "UserOwnerId");

            migrationBuilder.RenameColumn(
                name: "ConsultantDelegateId",
                table: "Processes",
                newName: "UserDelegateId");

            migrationBuilder.RenameIndex(
                name: "IX_Processes_ConsultantOwnerId",
                table: "Processes",
                newName: "IX_Processes_UserOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Processes_ConsultantDelegateId",
                table: "Processes",
                newName: "IX_Processes_UserDelegateId");

            migrationBuilder.RenameColumn(
                name: "ConsultantOwnerId",
                table: "OfferStages",
                newName: "UserOwnerId");

            migrationBuilder.RenameColumn(
                name: "ConsultantDelegateId",
                table: "OfferStages",
                newName: "UserDelegateId");

            migrationBuilder.RenameIndex(
                name: "IX_OfferStages_ConsultantOwnerId",
                table: "OfferStages",
                newName: "IX_OfferStages_UserOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_OfferStages_ConsultantDelegateId",
                table: "OfferStages",
                newName: "IX_OfferStages_UserDelegateId");

            migrationBuilder.RenameColumn(
                name: "ConsultantOwnerId",
                table: "HrStages",
                newName: "UserOwnerId");

            migrationBuilder.RenameColumn(
                name: "ConsultantDelegateId",
                table: "HrStages",
                newName: "UserDelegateId");

            migrationBuilder.RenameIndex(
                name: "IX_HrStages_ConsultantOwnerId",
                table: "HrStages",
                newName: "IX_HrStages_UserOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_HrStages_ConsultantDelegateId",
                table: "HrStages",
                newName: "IX_HrStages_UserDelegateId");

            migrationBuilder.RenameColumn(
                name: "RecruiterId",
                table: "Employees",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_RecruiterId",
                table: "Employees",
                newName: "IX_Employees_UserId");

            migrationBuilder.RenameColumn(
                name: "ConsultantOwnerId",
                table: "ClientStages",
                newName: "UserOwnerId");

            migrationBuilder.RenameColumn(
                name: "ConsultantDelegateId",
                table: "ClientStages",
                newName: "UserDelegateId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientStages_ConsultantOwnerId",
                table: "ClientStages",
                newName: "IX_ClientStages_UserOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientStages_ConsultantDelegateId",
                table: "ClientStages",
                newName: "IX_ClientStages_UserDelegateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientStages_Users_UserDelegateId",
                table: "ClientStages",
                column: "UserDelegateId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientStages_Users_UserOwnerId",
                table: "ClientStages",
                column: "UserOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Users_UserId",
                table: "Employees",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HrStages_Users_UserDelegateId",
                table: "HrStages",
                column: "UserDelegateId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HrStages_Users_UserOwnerId",
                table: "HrStages",
                column: "UserOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferStages_Users_UserDelegateId",
                table: "OfferStages",
                column: "UserDelegateId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferStages_Users_UserOwnerId",
                table: "OfferStages",
                column: "UserOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Processes_Users_UserDelegateId",
                table: "Processes",
                column: "UserDelegateId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Processes_Users_UserOwnerId",
                table: "Processes",
                column: "UserOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Users_UserId",
                table: "Reservation",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_Users_UserDelegateId",
                table: "Stages",
                column: "UserDelegateId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_Users_UserOwnerId",
                table: "Stages",
                column: "UserOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_UserId",
                table: "Tasks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalStages_Users_UserDelegateId",
                table: "TechnicalStages",
                column: "UserDelegateId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalStages_Users_UserOwnerId",
                table: "TechnicalStages",
                column: "UserOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientStages_Users_UserDelegateId",
                table: "ClientStages");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientStages_Users_UserOwnerId",
                table: "ClientStages");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Users_UserId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_HrStages_Users_UserDelegateId",
                table: "HrStages");

            migrationBuilder.DropForeignKey(
                name: "FK_HrStages_Users_UserOwnerId",
                table: "HrStages");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferStages_Users_UserDelegateId",
                table: "OfferStages");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferStages_Users_UserOwnerId",
                table: "OfferStages");

            migrationBuilder.DropForeignKey(
                name: "FK_Processes_Users_UserDelegateId",
                table: "Processes");

            migrationBuilder.DropForeignKey(
                name: "FK_Processes_Users_UserOwnerId",
                table: "Processes");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Users_UserId",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Stages_Users_UserDelegateId",
                table: "Stages");

            migrationBuilder.DropForeignKey(
                name: "FK_Stages_Users_UserOwnerId",
                table: "Stages");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_UserId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalStages_Users_UserDelegateId",
                table: "TechnicalStages");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalStages_Users_UserOwnerId",
                table: "TechnicalStages");

            migrationBuilder.RenameColumn(
                name: "UserOwnerId",
                table: "TechnicalStages",
                newName: "ConsultantOwnerId");

            migrationBuilder.RenameColumn(
                name: "UserDelegateId",
                table: "TechnicalStages",
                newName: "ConsultantDelegateId");

            migrationBuilder.RenameIndex(
                name: "IX_TechnicalStages_UserOwnerId",
                table: "TechnicalStages",
                newName: "IX_TechnicalStages_ConsultantOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_TechnicalStages_UserDelegateId",
                table: "TechnicalStages",
                newName: "IX_TechnicalStages_ConsultantDelegateId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Tasks",
                newName: "ConsultantId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_UserId",
                table: "Tasks",
                newName: "IX_Tasks_ConsultantId");

            migrationBuilder.RenameColumn(
                name: "UserOwnerId",
                table: "Stages",
                newName: "ConsultantOwnerId");

            migrationBuilder.RenameColumn(
                name: "UserDelegateId",
                table: "Stages",
                newName: "ConsultantDelegateId");

            migrationBuilder.RenameIndex(
                name: "IX_Stages_UserOwnerId",
                table: "Stages",
                newName: "IX_Stages_ConsultantOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Stages_UserDelegateId",
                table: "Stages",
                newName: "IX_Stages_ConsultantDelegateId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Reservation",
                newName: "RecruiterId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_UserId",
                table: "Reservation",
                newName: "IX_Reservation_RecruiterId");

            migrationBuilder.RenameColumn(
                name: "UserOwnerId",
                table: "Processes",
                newName: "ConsultantOwnerId");

            migrationBuilder.RenameColumn(
                name: "UserDelegateId",
                table: "Processes",
                newName: "ConsultantDelegateId");

            migrationBuilder.RenameIndex(
                name: "IX_Processes_UserOwnerId",
                table: "Processes",
                newName: "IX_Processes_ConsultantOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Processes_UserDelegateId",
                table: "Processes",
                newName: "IX_Processes_ConsultantDelegateId");

            migrationBuilder.RenameColumn(
                name: "UserOwnerId",
                table: "OfferStages",
                newName: "ConsultantOwnerId");

            migrationBuilder.RenameColumn(
                name: "UserDelegateId",
                table: "OfferStages",
                newName: "ConsultantDelegateId");

            migrationBuilder.RenameIndex(
                name: "IX_OfferStages_UserOwnerId",
                table: "OfferStages",
                newName: "IX_OfferStages_ConsultantOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_OfferStages_UserDelegateId",
                table: "OfferStages",
                newName: "IX_OfferStages_ConsultantDelegateId");

            migrationBuilder.RenameColumn(
                name: "UserOwnerId",
                table: "HrStages",
                newName: "ConsultantOwnerId");

            migrationBuilder.RenameColumn(
                name: "UserDelegateId",
                table: "HrStages",
                newName: "ConsultantDelegateId");

            migrationBuilder.RenameIndex(
                name: "IX_HrStages_UserOwnerId",
                table: "HrStages",
                newName: "IX_HrStages_ConsultantOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_HrStages_UserDelegateId",
                table: "HrStages",
                newName: "IX_HrStages_ConsultantDelegateId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Employees",
                newName: "RecruiterId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                newName: "IX_Employees_RecruiterId");

            migrationBuilder.RenameColumn(
                name: "UserOwnerId",
                table: "ClientStages",
                newName: "ConsultantOwnerId");

            migrationBuilder.RenameColumn(
                name: "UserDelegateId",
                table: "ClientStages",
                newName: "ConsultantDelegateId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientStages_UserOwnerId",
                table: "ClientStages",
                newName: "IX_ClientStages_ConsultantOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientStages_UserDelegateId",
                table: "ClientStages",
                newName: "IX_ClientStages_ConsultantDelegateId");

            migrationBuilder.AddColumn<int>(
                name: "RecruiterId",
                table: "Candidates",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Consultants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdditionalInformation = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    EmailAddress = table.Column<string>(nullable: true),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Version = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultants", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_RecruiterId",
                table: "Candidates",
                column: "RecruiterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_Consultants_RecruiterId",
                table: "Candidates",
                column: "RecruiterId",
                principalTable: "Consultants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientStages_Consultants_ConsultantDelegateId",
                table: "ClientStages",
                column: "ConsultantDelegateId",
                principalTable: "Consultants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientStages_Consultants_ConsultantOwnerId",
                table: "ClientStages",
                column: "ConsultantOwnerId",
                principalTable: "Consultants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Consultants_RecruiterId",
                table: "Employees",
                column: "RecruiterId",
                principalTable: "Consultants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HrStages_Consultants_ConsultantDelegateId",
                table: "HrStages",
                column: "ConsultantDelegateId",
                principalTable: "Consultants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HrStages_Consultants_ConsultantOwnerId",
                table: "HrStages",
                column: "ConsultantOwnerId",
                principalTable: "Consultants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferStages_Consultants_ConsultantDelegateId",
                table: "OfferStages",
                column: "ConsultantDelegateId",
                principalTable: "Consultants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferStages_Consultants_ConsultantOwnerId",
                table: "OfferStages",
                column: "ConsultantOwnerId",
                principalTable: "Consultants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Processes_Consultants_ConsultantDelegateId",
                table: "Processes",
                column: "ConsultantDelegateId",
                principalTable: "Consultants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Processes_Consultants_ConsultantOwnerId",
                table: "Processes",
                column: "ConsultantOwnerId",
                principalTable: "Consultants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Consultants_RecruiterId",
                table: "Reservation",
                column: "RecruiterId",
                principalTable: "Consultants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_Consultants_ConsultantDelegateId",
                table: "Stages",
                column: "ConsultantDelegateId",
                principalTable: "Consultants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_Consultants_ConsultantOwnerId",
                table: "Stages",
                column: "ConsultantOwnerId",
                principalTable: "Consultants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Consultants_ConsultantId",
                table: "Tasks",
                column: "ConsultantId",
                principalTable: "Consultants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalStages_Consultants_ConsultantDelegateId",
                table: "TechnicalStages",
                column: "ConsultantDelegateId",
                principalTable: "Consultants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalStages_Consultants_ConsultantOwnerId",
                table: "TechnicalStages",
                column: "ConsultantOwnerId",
                principalTable: "Consultants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
