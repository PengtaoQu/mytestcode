using Microsoft.EntityFrameworkCore.Migrations;

namespace MigrationEf.Migrations
{
    public partial class inV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "tripacti",
                newName: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "tripacti",
                newName: "ID");
        }
    }
}
