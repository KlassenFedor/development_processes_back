using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dev_processes_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddLinkBetweenInterviewsAndStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "Interviews",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Interviews_StudentId",
                table: "Interviews",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interviews_Users_StudentId",
                table: "Interviews",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interviews_Users_StudentId",
                table: "Interviews");

            migrationBuilder.DropIndex(
                name: "IX_Interviews_StudentId",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Interviews");
        }
    }
}
