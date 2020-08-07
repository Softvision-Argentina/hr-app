// <copyright file="20200506194059_Sixth.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Sixth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalRemuneration",
                table: "OfferStages");

            migrationBuilder.AddColumn<int>(
                name: "RemunerationOffer",
                table: "OfferStages",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemunerationOffer",
                table: "OfferStages");

            migrationBuilder.AddColumn<string>(
                name: "FinalRemuneration",
                table: "OfferStages",
                nullable: true);
        }
    }
}
