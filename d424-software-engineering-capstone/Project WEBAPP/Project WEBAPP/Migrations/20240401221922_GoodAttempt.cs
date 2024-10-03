using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_WEBAPP.Migrations
{
    /// <inheritdoc />
    public partial class GoodAttempt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeTimeLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<string>(type: "TEXT", nullable: false),
                    ClockInTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ClockOutTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTimeLogs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeTimeLogs");
        }
    }
}
