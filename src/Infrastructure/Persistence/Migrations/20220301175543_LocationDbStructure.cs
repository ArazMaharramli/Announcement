using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Persistence.Migrations
{
    public partial class LocationDbStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_Latitude",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Address_Longtitude",
                table: "Rooms");

            migrationBuilder.AddColumn<Point>(
                name: "Address_Location",
                table: "Rooms",
                type: "geography",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_Location",
                table: "Rooms");

            migrationBuilder.AddColumn<double>(
                name: "Address_Latitude",
                table: "Rooms",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Address_Longtitude",
                table: "Rooms",
                type: "float",
                nullable: true);
        }
    }
}
