using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarriorSalesAPI.Migrations
{
    public partial class AddDescriptionColumnAtSaleItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SaleItems",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "SaleItems");
        }
    }
}
