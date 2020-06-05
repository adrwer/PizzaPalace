using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaPalace.Migrations
{
    public partial class DeletedPricesColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prices",
                table: "Order");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Prices",
                table: "Order",
                nullable: true);
        }
    }
}
