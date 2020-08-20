using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace HTTAPI.Migrations
{
    public partial class addedRequestnumbertohealthtrack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SampleDate",
                table: "HealthTrack");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfTravel",
                table: "HealthTrack",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RequestNumber",
                table: "HealthTrack",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfTravel",
                table: "HealthTrack");

            migrationBuilder.DropColumn(
                name: "RequestNumber",
                table: "HealthTrack");

            migrationBuilder.AddColumn<DateTime>(
                name: "SampleDate",
                table: "HealthTrack",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
