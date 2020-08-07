// <copyright file="20200429142405_Fourth.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Fourth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalInformation",
                table: "Candidates");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInformation",
                table: "HrStages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalInformation",
                table: "HrStages");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInformation",
                table: "Candidates",
                nullable: true);
        }
    }
}
