using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProgrammingTournament.Migrations
{
    public partial class AddTournamentsEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    TournamentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    DueDate = table.Column<DateTime>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    OwnerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.TournamentId);
                    table.ForeignKey(
                        name: "FK_Tournaments_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentTournaments",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(nullable: false),
                    TournamentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTournaments", x => new { x.ApplicationUserId, x.TournamentId });
                    table.ForeignKey(
                        name: "FK_StudentTournaments_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentTournaments_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "TournamentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TournamentTasks",
                columns: table => new
                {
                    TournamentTaskId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    DueDate = table.Column<DateTime>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    MaxAttempt = table.Column<int>(nullable: false),
                    TournamentId = table.Column<int>(nullable: true),
                    InputFilePath = table.Column<string>(nullable: true),
                    OwnerId = table.Column<string>(nullable: true),
                    Timeout = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentTasks", x => x.TournamentTaskId);
                    table.ForeignKey(
                        name: "FK_TournamentTasks_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentTasks_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "TournamentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupportedProgrammingLanguages",
                columns: table => new
                {
                    SupportedProgrammingLanguageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    TournamentTaskId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportedProgrammingLanguages", x => x.SupportedProgrammingLanguageId);
                    table.ForeignKey(
                        name: "FK_SupportedProgrammingLanguages_TournamentTasks_TournamentTaskId",
                        column: x => x.TournamentTaskId,
                        principalTable: "TournamentTasks",
                        principalColumn: "TournamentTaskId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentTaskAssignmentResults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TaskTournamentTaskId = table.Column<int>(nullable: true),
                    ProcessResultId = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "TournamentTaskAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    IsPassed = table.Column<bool>(nullable: false),
                    Attempts = table.Column<int>(nullable: false),
                    TaskTournamentTaskId = table.Column<int>(nullable: true),
                    LastAttemptedAt = table.Column<DateTime>(nullable: false),
                    WorkDir = table.Column<string>(nullable: true),
                    ResultId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentTaskAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentTaskAssignments_TournamentTaskAssignmentResults_ResultId",
                        column: x => x.ResultId,
                        principalTable: "TournamentTaskAssignmentResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentTaskAssignments_TournamentTasks_TaskTournamentTaskId",
                        column: x => x.TaskTournamentTaskId,
                        principalTable: "TournamentTasks",
                        principalColumn: "TournamentTaskId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentTaskAssignments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentTournaments_TournamentId",
                table: "StudentTournaments",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportedProgrammingLanguages_TournamentTaskId",
                table: "SupportedProgrammingLanguages",
                column: "TournamentTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_OwnerId",
                table: "Tournaments",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTaskAssignmentResults_TaskTournamentTaskId",
                table: "TournamentTaskAssignmentResults",
                column: "TaskTournamentTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTaskAssignments_ResultId",
                table: "TournamentTaskAssignments",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTaskAssignments_TaskTournamentTaskId",
                table: "TournamentTaskAssignments",
                column: "TaskTournamentTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTaskAssignments_UserId",
                table: "TournamentTaskAssignments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTasks_OwnerId",
                table: "TournamentTasks",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTasks_TournamentId",
                table: "TournamentTasks",
                column: "TournamentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentTournaments");

            migrationBuilder.DropTable(
                name: "SupportedProgrammingLanguages");

            migrationBuilder.DropTable(
                name: "TournamentTaskAssignments");

            migrationBuilder.DropTable(
                name: "TournamentTaskAssignmentResults");

            migrationBuilder.DropTable(
                name: "TournamentTasks");

            migrationBuilder.DropTable(
                name: "Tournaments");
        }
    }
}
