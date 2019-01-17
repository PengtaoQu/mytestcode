using Microsoft.EntityFrameworkCore.Migrations;

namespace MigrationEf.Migrations
{
    public partial class up2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "courseID",
                table: "students",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_students_courseID",
                table: "students",
                column: "courseID");

            migrationBuilder.AddForeignKey(
                name: "FK_students_courses_courseID",
                table: "students",
                column: "courseID",
                principalTable: "courses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_students_courses_courseID",
                table: "students");

            migrationBuilder.DropIndex(
                name: "IX_students_courseID",
                table: "students");

            migrationBuilder.DropColumn(
                name: "courseID",
                table: "students");
        }
    }
}
