using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dev_processes_backend.Migrations
{
    /// <inheritdoc />
    public partial class MoveDescriptionFromInterviewStateToInterview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "InterviewStates");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Interviews",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Interviews");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "InterviewStates",
                type: "text",
                nullable: true);
        }
    }
}
