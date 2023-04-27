using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dev_processes_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddCreateDateTimeForSoftDeletableEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDateTime",
                table: "Vacancies",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDateTime",
                table: "UserRoles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDateTime",
                table: "Vacancies");

            migrationBuilder.DropColumn(
                name: "CreateDateTime",
                table: "UserRoles");
        }
    }
}
