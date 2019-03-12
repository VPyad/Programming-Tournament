using Microsoft.EntityFrameworkCore.Migrations;

namespace Programming_Tournament.Data.Migrations
{
    public partial class RemoveTablesFromContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Faculties_FacultyId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Lecterns_LecternId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Сurriculums_СurriculumId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Faculties");

            migrationBuilder.DropTable(
                name: "Lecterns");

            migrationBuilder.DropTable(
                name: "Сurriculums");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FacultyId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LecternId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_СurriculumId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DegreeType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FacultyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LecternId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "СurriculumId",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DegreeType",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "FacultyId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LecternId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "СurriculumId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    FacultyId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.FacultyId);
                });

            migrationBuilder.CreateTable(
                name: "Lecterns",
                columns: table => new
                {
                    LecternId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecterns", x => x.LecternId);
                });

            migrationBuilder.CreateTable(
                name: "Сurriculums",
                columns: table => new
                {
                    СurriculumId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Сurriculums", x => x.СurriculumId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FacultyId",
                table: "AspNetUsers",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LecternId",
                table: "AspNetUsers",
                column: "LecternId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_СurriculumId",
                table: "AspNetUsers",
                column: "СurriculumId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Faculties_FacultyId",
                table: "AspNetUsers",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "FacultyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Lecterns_LecternId",
                table: "AspNetUsers",
                column: "LecternId",
                principalTable: "Lecterns",
                principalColumn: "LecternId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Сurriculums_СurriculumId",
                table: "AspNetUsers",
                column: "СurriculumId",
                principalTable: "Сurriculums",
                principalColumn: "СurriculumId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
