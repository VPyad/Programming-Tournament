using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProgrammingTournament.Migrations
{
    public partial class DeleteAssignmentResultEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentTaskAssignments_TournamentTaskAssignmentResults_ResultId",
                table: "TournamentTaskAssignments");

            migrationBuilder.DropTable(
                name: "TournamentTaskAssignmentResults");

            migrationBuilder.DropIndex(
                name: "IX_TournamentTaskAssignments_ResultId",
                table: "TournamentTaskAssignments");

            migrationBuilder.DropColumn(
                name: "ResultId",
                table: "TournamentTaskAssignments");

            migrationBuilder.AddColumn<string>(
                name: "ProcessResultId",
                table: "TournamentTaskAssignments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessResultId",
                table: "TournamentTaskAssignments");

            migrationBuilder.AddColumn<int>(
                name: "ResultId",
                table: "TournamentTaskAssignments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TournamentTaskAssignmentResults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProcessResultId = table.Column<int>(nullable: false),
                    TaskTournamentTaskId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentTaskAssignmentResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentTaskAssignmentResults_TournamentTasks_TaskTournamentTaskId",
                        column: x => x.TaskTournamentTaskId,
                        principalTable: "TournamentTasks",
                        principalColumn: "TournamentTaskId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTaskAssignments_ResultId",
                table: "TournamentTaskAssignments",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTaskAssignmentResults_TaskTournamentTaskId",
                table: "TournamentTaskAssignmentResults",
                column: "TaskTournamentTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentTaskAssignments_TournamentTaskAssignmentResults_ResultId",
                table: "TournamentTaskAssignments",
                column: "ResultId",
                principalTable: "TournamentTaskAssignmentResults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
