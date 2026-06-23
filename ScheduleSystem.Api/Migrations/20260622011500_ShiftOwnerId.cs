using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ScheduleSystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class ShiftOwnerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove all existing shifts (seeds + any user-created) before adding NOT NULL OwnerId.
            // EmployeeAvailableShifts must be cleared first due to the FK Restrict constraint.
            migrationBuilder.Sql("DELETE FROM [EmployeeAvailableShifts]");
            migrationBuilder.Sql("DELETE FROM [Shifts]");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Shifts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_OwnerId",
                table: "Shifts",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Users_OwnerId",
                table: "Shifts",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Users_OwnerId",
                table: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_Shifts_OwnerId",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Shifts");

            migrationBuilder.InsertData(
                table: "Shifts",
                columns: new[] { "Id", "Color", "EndTime", "Hours", "Name", "StartTime" },
                values: new object[,]
                {
                    { 1, "#F6AD55", new TimeSpan(0, 17, 0, 0, 0), 9, "8am - 5pm", new TimeSpan(0, 8, 0, 0, 0) },
                    { 2, "#68D391", new TimeSpan(0, 18, 0, 0, 0), 9, "9am - 6pm", new TimeSpan(0, 9, 0, 0, 0) },
                    { 3, "#76E4F7", new TimeSpan(0, 21, 0, 0, 0), 9, "12pm - 9pm", new TimeSpan(0, 12, 0, 0, 0) },
                    { 4, "#B794F4", new TimeSpan(0, 17, 0, 0, 0), 9, "Outlet", new TimeSpan(0, 8, 0, 0, 0) }
                });
        }
    }
}
