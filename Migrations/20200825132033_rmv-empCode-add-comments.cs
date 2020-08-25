using Microsoft.EntityFrameworkCore.Migrations;

namespace HTTAPI.Migrations
{
    public partial class rmvempCodeaddcomments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeCode",
                table: "ComeToOfficeRequest");

            migrationBuilder.AddColumn<string>(
                name: "HRComments",
                table: "ComeToOfficeRequest",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HRComments",
                table: "ComeToOfficeRequest");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeCode",
                table: "ComeToOfficeRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
