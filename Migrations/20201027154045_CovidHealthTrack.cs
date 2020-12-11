using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HTTAPI.Migrations
{
    public partial class CovidHealthTrack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CovidHealthTrack",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    DateOfSymptoms = table.Column<DateTime>(nullable: false),
                    OfficeLastDay = table.Column<DateTime>(nullable: false),
                    CovidConfirmationDate = table.Column<DateTime>(nullable: false),
                    OthersInfectedInFamily = table.Column<bool>(nullable: false),
                    FamilyMembersCount = table.Column<int>(nullable: false),
                    HospitalizationNeed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CovidHealthTrack", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CovidHealthTrack_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CovidHealthTrack_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CovidHealthTrack_EmployeeId",
                table: "CovidHealthTrack",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CovidHealthTrack_LocationId",
                table: "CovidHealthTrack",
                column: "LocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CovidHealthTrack");
        }
    }
}
