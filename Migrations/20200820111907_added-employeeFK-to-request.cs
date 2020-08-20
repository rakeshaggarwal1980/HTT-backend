using Microsoft.EntityFrameworkCore.Migrations;

namespace HTTAPI.Migrations
{
    public partial class addedemployeeFKtorequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "ComeToOfficeRequest",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ComeToOfficeRequest_EmployeeId",
                table: "ComeToOfficeRequest",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComeToOfficeRequest_Employee_EmployeeId",
                table: "ComeToOfficeRequest",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComeToOfficeRequest_Employee_EmployeeId",
                table: "ComeToOfficeRequest");

            migrationBuilder.DropIndex(
                name: "IX_ComeToOfficeRequest_EmployeeId",
                table: "ComeToOfficeRequest");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "ComeToOfficeRequest");
        }
    }
}
