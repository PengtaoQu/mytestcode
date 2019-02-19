using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MigrationEf.Migrations
{
    public partial class inIN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tripacti",
                table: "tripacti");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "tripacti",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tripacti",
                table: "tripacti",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_tripacti_tripID",
                table: "tripacti",
                column: "tripID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tripacti",
                table: "tripacti");

            migrationBuilder.DropIndex(
                name: "IX_tripacti_tripID",
                table: "tripacti");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "tripacti");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tripacti",
                table: "tripacti",
                columns: new[] { "tripID", "activityID" });
        }
    }
}
