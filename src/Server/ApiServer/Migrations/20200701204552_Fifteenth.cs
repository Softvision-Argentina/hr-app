// <copyright file="20200701204552_Fifteenth.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Fifteenth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReaddressStatusId",
                table: "TechnicalStages",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReaddressStatusId",
                table: "PreOfferStages",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReaddressStatusId",
                table: "HrStages",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReaddressStatusId",
                table: "ClientStages",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReaddressReasonTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReaddressReasonTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReaddressReasons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: false),
                    TypeId = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReaddressReasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReaddressReasons_ReaddressReasonTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ReaddressReasonTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReaddressStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    FromStatus = table.Column<int>(nullable: false),
                    ToStatus = table.Column<int>(nullable: false),
                    ReaddressReasonId = table.Column<int>(nullable: true),
                    Feedback = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReaddressStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReaddressStatus_ReaddressReasons_ReaddressReasonId",
                        column: x => x.ReaddressReasonId,
                        principalTable: "ReaddressReasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalStages_ReaddressStatusId",
                table: "TechnicalStages",
                column: "ReaddressStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PreOfferStages_ReaddressStatusId",
                table: "PreOfferStages",
                column: "ReaddressStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_HrStages_ReaddressStatusId",
                table: "HrStages",
                column: "ReaddressStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientStages_ReaddressStatusId",
                table: "ClientStages",
                column: "ReaddressStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ReaddressReasons_TypeId",
                table: "ReaddressReasons",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReaddressStatus_ReaddressReasonId",
                table: "ReaddressStatus",
                column: "ReaddressReasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientStages_ReaddressStatus_ReaddressStatusId",
                table: "ClientStages",
                column: "ReaddressStatusId",
                principalTable: "ReaddressStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HrStages_ReaddressStatus_ReaddressStatusId",
                table: "HrStages",
                column: "ReaddressStatusId",
                principalTable: "ReaddressStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PreOfferStages_ReaddressStatus_ReaddressStatusId",
                table: "PreOfferStages",
                column: "ReaddressStatusId",
                principalTable: "ReaddressStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalStages_ReaddressStatus_ReaddressStatusId",
                table: "TechnicalStages",
                column: "ReaddressStatusId",
                principalTable: "ReaddressStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientStages_ReaddressStatus_ReaddressStatusId",
                table: "ClientStages");

            migrationBuilder.DropForeignKey(
                name: "FK_HrStages_ReaddressStatus_ReaddressStatusId",
                table: "HrStages");

            migrationBuilder.DropForeignKey(
                name: "FK_PreOfferStages_ReaddressStatus_ReaddressStatusId",
                table: "PreOfferStages");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalStages_ReaddressStatus_ReaddressStatusId",
                table: "TechnicalStages");

            migrationBuilder.DropTable(
                name: "ReaddressStatus");

            migrationBuilder.DropTable(
                name: "ReaddressReasons");

            migrationBuilder.DropTable(
                name: "ReaddressReasonTypes");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalStages_ReaddressStatusId",
                table: "TechnicalStages");

            migrationBuilder.DropIndex(
                name: "IX_PreOfferStages_ReaddressStatusId",
                table: "PreOfferStages");

            migrationBuilder.DropIndex(
                name: "IX_HrStages_ReaddressStatusId",
                table: "HrStages");

            migrationBuilder.DropIndex(
                name: "IX_ClientStages_ReaddressStatusId",
                table: "ClientStages");

            migrationBuilder.DropColumn(
                name: "ReaddressStatusId",
                table: "TechnicalStages");

            migrationBuilder.DropColumn(
                name: "ReaddressStatusId",
                table: "PreOfferStages");

            migrationBuilder.DropColumn(
                name: "ReaddressStatusId",
                table: "HrStages");

            migrationBuilder.DropColumn(
                name: "ReaddressStatusId",
                table: "ClientStages");
        }
    }
}
