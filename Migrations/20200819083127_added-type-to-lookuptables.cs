using Microsoft.EntityFrameworkCore.Migrations;

namespace HTTAPI.Migrations
{
    public partial class addedtypetolookuptables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Zone",
                type: "nvarchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Symptom",
                type: "nvarchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Location",
                type: "nvarchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Location",
                keyColumn: "Id",
                keyValue: 1,
                column: "Type",
                value: "CheckBox");

            migrationBuilder.UpdateData(
                table: "Location",
                keyColumn: "Id",
                keyValue: 2,
                column: "Type",
                value: "CheckBox");

            migrationBuilder.UpdateData(
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 1,
                column: "Type",
                value: "CheckBox");

            migrationBuilder.UpdateData(
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 2,
                column: "Type",
                value: "CheckBox");

            migrationBuilder.UpdateData(
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 3,
                column: "Type",
                value: "CheckBox");

            migrationBuilder.UpdateData(
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 4,
                column: "Type",
                value: "CheckBox");

            migrationBuilder.UpdateData(
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 5,
                column: "Type",
                value: "CheckBox");

            migrationBuilder.UpdateData(
                table: "Zone",
                keyColumn: "Id",
                keyValue: 1,
                column: "Type",
                value: "CheckBox");

            migrationBuilder.UpdateData(
                table: "Zone",
                keyColumn: "Id",
                keyValue: 2,
                column: "Type",
                value: "CheckBox");

            migrationBuilder.UpdateData(
                table: "Zone",
                keyColumn: "Id",
                keyValue: 3,
                column: "Type",
                value: "CheckBox");

            migrationBuilder.UpdateData(
                table: "Zone",
                keyColumn: "Id",
                keyValue: 4,
                column: "Type",
                value: "CheckBox");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Zone");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Symptom");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Location");
        }
    }
}
