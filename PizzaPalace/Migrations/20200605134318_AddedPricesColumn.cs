using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaPalace.Migrations
{
    public partial class AddedPricesColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Prices",
                table: "Order",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prices",
                table: "Order");
        }
    }
}
