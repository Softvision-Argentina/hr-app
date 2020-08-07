// <copyright file="20200617161406_Sixteenth.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Sixteenth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Bonus",
                table: "PreOffer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "PreOffer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
            name: "TentativeStartDate",
            table: "PreOffer",
            nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bonus",
                table: "PreOffer");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "PreOffer");

            migrationBuilder.DropColumn(
            name: "TentativeStartDate",
            table: "PreOffer");
        }
    }
}
