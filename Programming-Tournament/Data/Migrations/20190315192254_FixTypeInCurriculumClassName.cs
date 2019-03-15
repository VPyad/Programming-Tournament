using Microsoft.EntityFrameworkCore.Migrations;

namespace Programming_Tournament.Data.Migrations
{
    public partial class FixTypeInCurriculumClassName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Сurriculums_СurriculumId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentApplications_Сurriculums_СurriculumId",
                table: "StudentApplications");

            migrationBuilder.RenameColumn(
                name: "СurriculumId",
                table: "Сurriculums",
                newName: "CurriculumId");

            migrationBuilder.RenameColumn(
                name: "СurriculumId",
                table: "StudentApplications",
                newName: "СurriculumCurriculumId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentApplications_СurriculumId",
                table: "StudentApplications",
                newName: "IX_StudentApplications_СurriculumCurriculumId");

            migrationBuilder.RenameColumn(
                name: "СurriculumId",
                table: "AspNetUsers",
                newName: "СurriculumCurriculumId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_СurriculumId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_СurriculumCurriculumId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Сurriculums_СurriculumCurriculumId",
                table: "AspNetUsers",
                column: "СurriculumCurriculumId",
                principalTable: "Сurriculums",
                principalColumn: "CurriculumId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentApplications_Сurriculums_СurriculumCurriculumId",
                table: "StudentApplications",
                column: "СurriculumCurriculumId",
                principalTable: "Сurriculums",
                principalColumn: "CurriculumId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Сurriculums_СurriculumCurriculumId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentApplications_Сurriculums_СurriculumCurriculumId",
                table: "StudentApplications");

            migrationBuilder.RenameColumn(
                name: "CurriculumId",
                table: "Сurriculums",
                newName: "СurriculumId");

            migrationBuilder.RenameColumn(
                name: "СurriculumCurriculumId",
                table: "StudentApplications",
                newName: "СurriculumId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentApplications_СurriculumCurriculumId",
                table: "StudentApplications",
                newName: "IX_StudentApplications_СurriculumId");

            migrationBuilder.RenameColumn(
                name: "СurriculumCurriculumId",
                table: "AspNetUsers",
                newName: "СurriculumId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_СurriculumCurriculumId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_СurriculumId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Сurriculums_СurriculumId",
                table: "AspNetUsers",
                column: "СurriculumId",
                principalTable: "Сurriculums",
                principalColumn: "СurriculumId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentApplications_Сurriculums_СurriculumId",
                table: "StudentApplications",
                column: "СurriculumId",
                principalTable: "Сurriculums",
                principalColumn: "СurriculumId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
