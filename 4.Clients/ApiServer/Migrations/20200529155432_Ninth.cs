﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiServer.Migrations
{
    public partial class Ninth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HealthInsurance",
                table: "PreOfferStages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "PreOfferStages",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HealthInsurance",
                table: "OfferStages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "OfferStages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HealthInsurance",
                table: "PreOfferStages");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "PreOfferStages");

            migrationBuilder.DropColumn(
                name: "HealthInsurance",
                table: "OfferStages");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "OfferStages");
        }
    }
}
