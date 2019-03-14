using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Programming_Tournament.Data.Migrations
{
    public partial class AddApplicationsEnteties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LecturerApplications",
                columns: table => new
                {
                    FirstName = table.Column<string>(nullable: true),
                    SecondName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    DocNo = table.Column<int>(nullable: true),
                    FacultyId = table.Column<long>(nullable: false),
                    LecternId = table.Column<long>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    IsRegistered = table.Column<bool>(nullable: false),
                    LecturerApplicationId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LecturerApplications", x => x.LecturerApplicationId);
                });

            migrationBuilder.CreateTable(
                name: "StudentApplications",
                columns: table => new
                {
                    FirstName = table.Column<string>(nullable: true),
                    SecondName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    DocNo = table.Column<int>(nullable: true),
                    FacultyId = table.Column<long>(nullable: false),
                    LecternId = table.Column<long>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    IsRegistered = table.Column<bool>(nullable: false),
                    StudentApplicationId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DegreeType = table.Column<int>(nullable: false),
                    СurriculumId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentApplications", x => x.StudentApplicationId);
                    table.ForeignKey(
                        name: "FK_StudentApplications_Сurriculums_СurriculumId",
                        column: x => x.СurriculumId,
                        principalTable: "Сurriculums",
                        principalColumn: "СurriculumId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentApplications_СurriculumId",
                table: "StudentApplications",
                column: "СurriculumId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LecturerApplications");

            migrationBuilder.DropTable(
                name: "StudentApplications");
        }
    }
}
