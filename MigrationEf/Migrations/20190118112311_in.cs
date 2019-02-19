using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MigrationEf.Migrations
{
    public partial class @in : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "activities",
                columns: table => new
                {
                    activityID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activities", x => x.activityID);
                });

            migrationBuilder.CreateTable(
                name: "trips",
                columns: table => new
                {
                    tripID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nAME = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trips", x => x.tripID);
                });

            migrationBuilder.CreateTable(
                name: "tripacti",
                columns: table => new
                {
                    activityID = table.Column<int>(nullable: false),
                    tripID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tripacti", x => new { x.tripID, x.activityID });
                    table.ForeignKey(
                        name: "FK_tripacti_activities_activityID",
                        column: x => x.activityID,
                        principalTable: "activities",
                        principalColumn: "activityID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tripacti_trips_tripID",
                        column: x => x.tripID,
                        principalTable: "trips",
                        principalColumn: "tripID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tripacti_activityID",
                table: "tripacti",
                column: "activityID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tripacti");

            migrationBuilder.DropTable(
                name: "activities");

            migrationBuilder.DropTable(
                name: "trips");
        }
    }
}
