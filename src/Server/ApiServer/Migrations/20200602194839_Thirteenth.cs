// <copyright file="20200602194839_Thirteenth.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Thirteenth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SentEmail",
                table: "TechnicalStages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SentEmail",
                table: "HrStages",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentEmail",
                table: "TechnicalStages");

            migrationBuilder.DropColumn(
                name: "SentEmail",
                table: "HrStages");
        }
    }
}
