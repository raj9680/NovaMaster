using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Imm.DAL.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    CnfEmail = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    CountryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    Role = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsersManager",
                columns: table => new
                {
                    StudentId = table.Column<int>(nullable: false),
                    AgentId = table.Column<int>(nullable: false),
                    DOJ = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsersManager", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_AspNetUsersManager_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    StateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<int>(nullable: false),
                    StateName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.StateId);
                    table.ForeignKey(
                        name: "FK_State_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    CityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateId = table.Column<int>(nullable: false),
                    CityName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.CityId);
                    table.ForeignKey(
                        name: "FK_City_State_StateId",
                        column: x => x.StateId,
                        principalTable: "State",
                        principalColumn: "StateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetStudentsInfo",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    ContactNumber = table.Column<string>(nullable: true),
                    DOB = table.Column<DateTime>(nullable: true),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    CityId = table.Column<int>(nullable: false),
                    Zip = table.Column<int>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    PrimaryLanguage = table.Column<string>(nullable: true),
                    EnglishExamType = table.Column<string>(nullable: true),
                    Intake = table.Column<string>(nullable: true),
                    IntakeYear = table.Column<int>(nullable: true),
                    Program = table.Column<string>(nullable: true),
                    ProgramCollegePreference = table.Column<string>(nullable: true),
                    HighestEducation = table.Column<string>(nullable: true),
                    MastersEducationStartDate = table.Column<DateTime>(nullable: true),
                    MastersEducationEndDate = table.Column<DateTime>(nullable: true),
                    MastersEducationCompletionDate = table.Column<DateTime>(nullable: true),
                    MastersInstituteInfo = table.Column<string>(nullable: true),
                    MastersEducationPercentage = table.Column<string>(nullable: true),
                    MastersEducationMathsmarks = table.Column<string>(nullable: true),
                    MastersEducationEnglishMarks = table.Column<string>(nullable: true),
                    BachelorsEducationStartDate = table.Column<DateTime>(nullable: true),
                    BachelorsEducationEndDate = table.Column<DateTime>(nullable: true),
                    BachelorsEducationCompletionDate = table.Column<DateTime>(nullable: true),
                    BachelorsInstituteInfo = table.Column<string>(nullable: true),
                    BachelorsEducationPercentage = table.Column<string>(nullable: true),
                    BachelorsEducationMathsmarks = table.Column<string>(nullable: true),
                    BachelorsEducationEnglishMarks = table.Column<string>(nullable: true),
                    SecondaryEducationStartDate = table.Column<DateTime>(nullable: true),
                    SecondaryEducationEndDate = table.Column<DateTime>(nullable: true),
                    SecondaryEducationCompletionDate = table.Column<DateTime>(nullable: true),
                    SecondaryInstituteInfo = table.Column<string>(nullable: true),
                    SecondaryEducationPercentage = table.Column<string>(nullable: true),
                    SecondaryEducationMathsmarks = table.Column<string>(nullable: true),
                    SecondaryEducationEnglishMarks = table.Column<string>(nullable: true),
                    MatricEducationStartDate = table.Column<DateTime>(nullable: false),
                    MatricEducationEndDate = table.Column<DateTime>(nullable: false),
                    MatricEducationCompletionDate = table.Column<DateTime>(nullable: false),
                    MatricInstituteInfo = table.Column<string>(nullable: true),
                    MatricEducationPercentage = table.Column<string>(nullable: true),
                    MatricEducationMathsmarks = table.Column<string>(nullable: true),
                    MatricEducationEnglishMarks = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    JobTitle = table.Column<string>(nullable: true),
                    JobStartDate = table.Column<DateTime>(nullable: true),
                    JobEndDate = table.Column<DateTime>(nullable: true),
                    IsRefusedVisa = table.Column<bool>(nullable: false),
                    ExplainIfRefused = table.Column<string>(nullable: true),
                    HaveStudyPermitVisa = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetStudentsInfo", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_AspNetStudentsInfo_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetStudentsInfo_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsersInfo",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    DOB = table.Column<DateTime>(nullable: false),
                    ContactNumber = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    StudentSource = table.Column<string>(nullable: true),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    CityId = table.Column<int>(nullable: false),
                    PinCode = table.Column<string>(nullable: true),
                    About = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsersInfo", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_AspNetUsersInfo_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUsersInfo_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserDocs",
                columns: table => new
                {
                    DocId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    DocumentName = table.Column<string>(nullable: true),
                    DocumentURL = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    IsVerified = table.Column<bool>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    AspNetUsersInfoUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserDocs", x => x.DocId);
                    table.ForeignKey(
                        name: "FK_AspNetUserDocs_AspNetUsersInfo_AspNetUsersInfoUserId",
                        column: x => x.AspNetUsersInfoUserId,
                        principalTable: "AspNetUsersInfo",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "UserId", "CnfEmail", "Email", "FullName", "IsActive", "Password" },
                values: new object[] { 1, false, "admin@admin.com", "Raj Aryan", false, "admin@admin.com" });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "CountryId", "CountryName" },
                values: new object[] { 1, "India" });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "CountryId", "CountryName" },
                values: new object[] { 2, "United States" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "UserId", "Role" },
                values: new object[] { 1, "agent" });

            migrationBuilder.InsertData(
                table: "State",
                columns: new[] { "StateId", "CountryId", "StateName" },
                values: new object[,]
                {
                    { 1, 1, "Punjab" },
                    { 2, 1, "Uttar Pradesh" },
                    { 3, 2, "NY" }
                });

            migrationBuilder.InsertData(
                table: "City",
                columns: new[] { "CityId", "CityName", "StateId" },
                values: new object[] { 3, "Amritsar", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetStudentsInfo_CityId",
                table: "AspNetStudentsInfo",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserDocs_AspNetUsersInfoUserId",
                table: "AspNetUserDocs",
                column: "AspNetUsersInfoUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsersInfo_CityId",
                table: "AspNetUsersInfo",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_City_StateId",
                table: "City",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_State_CountryId",
                table: "State",
                column: "CountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetStudentsInfo");

            migrationBuilder.DropTable(
                name: "AspNetUserDocs");

            migrationBuilder.DropTable(
                name: "AspNetUsersManager");

            migrationBuilder.DropTable(
                name: "AspNetUsersInfo");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropTable(
                name: "Country");
        }
    }
}
