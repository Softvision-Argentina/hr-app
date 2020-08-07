// <copyright file="20200506192911_Fifth.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Fifth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bonus",
                table: "OfferStages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FinalRemuneration",
                table: "OfferStages",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Firstday",
                table: "OfferStages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "VacationDays",
                table: "OfferStages",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bonus",
                table: "OfferStages");

            migrationBuilder.DropColumn(
                name: "FinalRemuneration",
                table: "OfferStages");

            migrationBuilder.DropColumn(
                name: "Firstday",
                table: "OfferStages");

            migrationBuilder.DropColumn(
                name: "VacationDays",
                table: "OfferStages");
        }
    }
}
