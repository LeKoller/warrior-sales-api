using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarriorSalesAPI.Migrations
{
    public partial class ChangeColumnTypeToCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Products",
                newName: "Category");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Products",
                newName: "Type");
        }
    }
}
