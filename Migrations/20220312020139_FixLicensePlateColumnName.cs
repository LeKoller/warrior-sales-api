using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarriorSalesAPI.Migrations
{
    public partial class FixLicensePlateColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LicencePlate",
                table: "Teams",
                newName: "LicensePlate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LicensePlate",
                table: "Teams",
                newName: "LicencePlate");
        }
    }
}
