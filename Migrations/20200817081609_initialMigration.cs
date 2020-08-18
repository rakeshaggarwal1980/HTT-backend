using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace HTTAPI.Migrations
{
    public partial class initialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    Password = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    EmployeeCode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Symptom",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symptom", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zone",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zone", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HealthTrack",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ResidentialAddress = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    PreExistHealthIssue = table.Column<bool>(nullable: false),
                    ContactWithCovidPeople = table.Column<bool>(nullable: false),
                    TravelOustSideInLast15Days = table.Column<bool>(nullable: false),
                    SampleDate = table.Column<DateTime>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    ZoneId = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthTrack", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthTrack_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HealthTrack_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HealthTrack_Zone_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HealthTrackQuestionAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HealthTrackId = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthTrackQuestionAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthTrackQuestionAnswer_HealthTrack_HealthTrackId",
                        column: x => x.HealthTrackId,
                        principalTable: "HealthTrack",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HealthTrackQuestionAnswer_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HealthTrackSymptom",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HealthTrackId = table.Column<int>(nullable: false),
                    SymptomId = table.Column<int>(nullable: false),
                    value = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthTrackSymptom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthTrackSymptom_HealthTrack_HealthTrackId",
                        column: x => x.HealthTrackId,
                        principalTable: "HealthTrack",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HealthTrackSymptom_Symptom_SymptomId",
                        column: x => x.SymptomId,
                        principalTable: "Symptom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Location",
                columns: new[] { "Id", "Name", "Order", "Status" },
                values: new object[,]
                {
                    { 1, "Tricity (CHD/PKL/Mohali)", 1, 0 },
                    { 2, "Outside Tricity", 2, 0 }
                });

            migrationBuilder.InsertData(
                table: "Question",
                columns: new[] { "Id", "Name", "Order", "Status", "Type" },
                values: new object[,]
                {
                    { 1, "Please give a count of family members in your house?", 1, 0, "Input" },
                    { 2, "Is any Of them under 5 years or over 65 years in age?", 2, 0, "CheckBox" },
                    { 3, "Has any Of them presented Covid-19 related symptoms recently?", 3, 0, "CheckBox" },
                    { 4, "Has any Of them had any recent travel — abroad, inter-state or inter or district ? ", 4, 0, "CheckBox" }
                });

            migrationBuilder.InsertData(
                table: "Symptom",
                columns: new[] { "Id", "Name", "Order", "Status" },
                values: new object[,]
                {
                    { 1, "Fever", 1, 0 },
                    { 2, "Shortness Of Breath", 2, 0 },
                    { 3, "Dry Cough", 3, 0 },
                    { 4, "Running Nose", 4, 0 },
                    { 5, "Sore Throat", 5, 0 }
                });

            migrationBuilder.InsertData(
                table: "Zone",
                columns: new[] { "Id", "Name", "Order", "Status" },
                values: new object[,]
                {
                    { 1, "Containment Zone", 1, 0 },
                    { 2, "Red Zone", 2, 0 },
                    { 3, "Green Zone", 3, 0 },
                    { 4, "Orange Zone", 4, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_HealthTrack_EmployeeId",
                table: "HealthTrack",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthTrack_LocationId",
                table: "HealthTrack",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthTrack_ZoneId",
                table: "HealthTrack",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthTrackQuestionAnswer_HealthTrackId",
                table: "HealthTrackQuestionAnswer",
                column: "HealthTrackId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthTrackQuestionAnswer_QuestionId",
                table: "HealthTrackQuestionAnswer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthTrackSymptom_HealthTrackId",
                table: "HealthTrackSymptom",
                column: "HealthTrackId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthTrackSymptom_SymptomId",
                table: "HealthTrackSymptom",
                column: "SymptomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HealthTrackQuestionAnswer");

            migrationBuilder.DropTable(
                name: "HealthTrackSymptom");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "HealthTrack");

            migrationBuilder.DropTable(
                name: "Symptom");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Zone");
        }
    }
}
