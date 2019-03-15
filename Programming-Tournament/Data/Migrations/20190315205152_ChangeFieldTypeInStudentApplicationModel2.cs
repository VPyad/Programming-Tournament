using Microsoft.EntityFrameworkCore.Migrations;

namespace Programming_Tournament.Data.Migrations
{
    public partial class ChangeFieldTypeInStudentApplicationModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "СurriculumId",
                table: "StudentApplications",
                newName: "CurriculumId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurriculumId",
                table: "StudentApplications",
                newName: "СurriculumId");
        }
    }
}
