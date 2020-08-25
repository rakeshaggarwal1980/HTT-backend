using Microsoft.EntityFrameworkCore.Migrations;

namespace HTTAPI.Migrations
{
    public partial class changedseeddata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Location",
                keyColumn: "Id",
                keyValue: 1,
                column: "Type",
                value: "Radio");

            migrationBuilder.UpdateData(
                table: "Location",
                keyColumn: "Id",
                keyValue: 2,
                column: "Type",
                value: "Radio");

            migrationBuilder.UpdateData(
                table: "Question",
                keyColumn: "Id",
                keyValue: 2,
                column: "Type",
                value: "Radio");

            migrationBuilder.UpdateData(
                table: "Question",
                keyColumn: "Id",
                keyValue: 3,
                column: "Type",
                value: "Radio");

            migrationBuilder.UpdateData(
                table: "Question",
                keyColumn: "Id",
                keyValue: 4,
                column: "Type",
                value: "Radio");

            migrationBuilder.UpdateData(
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 1,
                column: "Type",
                value: "Radio");

            migrationBuilder.UpdateData(
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 2,
                column: "Type",
                value: "Radio");

            migrationBuilder.UpdateData(
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 3,
                column: "Type",
                value: "Radio");

            migrationBuilder.UpdateData(
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 4,
                column: "Type",
                value: "Radio");

            migrationBuilder.UpdateData(
                table: "Symptom",
                keyColumn: "Id",
                keyValue: 5,
                column: "Type",
                value: "Radio");

            migrationBuilder.UpdateData(
                table: "Zone",
                keyColumn: "Id",
                keyValue: 1,
                column: "Type",
                value: "Radio");

            migrationBuilder.UpdateData(
                table: "Zone",
                keyColumn: "Id",
                keyValue: 2,
                column: "Type",
                value: "Radio");

            migrationBuilder.UpdateData(
                table: "Zone",
                keyColumn: "Id",
                keyValue: 3,
                column: "Type",
                value: "Radio");

            migrationBuilder.UpdateData(
                table: "Zone",
                keyColumn: "Id",
                keyValue: 4,
                column: "Type",
                value: "Radio");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                table: "Question",
                keyColumn: "Id",
                keyValue: 2,
                column: "Type",
                value: "CheckBox");

            migrationBuilder.UpdateData(
                table: "Question",
                keyColumn: "Id",
                keyValue: 3,
                column: "Type",
                value: "CheckBox");

            migrationBuilder.UpdateData(
                table: "Question",
                keyColumn: "Id",
                keyValue: 4,
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
    }
}
