using Microsoft.EntityFrameworkCore.Migrations;

namespace ProgrammingTournament.Migrations
{
    public partial class AddExpectedFilePathFieldToTaskEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExpectedFilePath",
                table: "TournamentTasks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedFilePath",
                table: "TournamentTasks");
        }
    }
}
