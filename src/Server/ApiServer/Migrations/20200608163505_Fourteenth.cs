// <copyright file="20200608163505_Fourteenth.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Fourteenth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnglishLevel",
                table: "TechnicalStages",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnglishLevel",
                table: "TechnicalStages");
        }
    }
}
