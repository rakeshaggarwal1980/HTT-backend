using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace HTTAPI.Migrations
{
    public partial class addedtokentoemp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetToken",
                table: "Employee",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpires",
                table: "Employee",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetToken",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpires",
                table: "Employee");
        }
    }
}
