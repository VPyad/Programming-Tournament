using Microsoft.EntityFrameworkCore.Migrations;

namespace Programming_Tournament.Data.Migrations
{
    public partial class ChangeFieldTypeInStudentApplicationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentApplications_Сurriculums_СurriculumCurriculumId",
                table: "StudentApplications");

            migrationBuilder.DropIndex(
                name: "IX_StudentApplications_СurriculumCurriculumId",
                table: "StudentApplications");

            migrationBuilder.DropColumn(
                name: "СurriculumCurriculumId",
                table: "StudentApplications");

            migrationBuilder.AddColumn<int>(
                name: "СurriculumId",
                table: "StudentApplications",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "СurriculumId",
                table: "StudentApplications");

            migrationBuilder.AddColumn<int>(
                name: "СurriculumCurriculumId",
                table: "StudentApplications",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentApplications_СurriculumCurriculumId",
                table: "StudentApplications",
                column: "СurriculumCurriculumId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentApplications_Сurriculums_СurriculumCurriculumId",
                table: "StudentApplications",
                column: "СurriculumCurriculumId",
                principalTable: "Сurriculums",
                principalColumn: "CurriculumId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
