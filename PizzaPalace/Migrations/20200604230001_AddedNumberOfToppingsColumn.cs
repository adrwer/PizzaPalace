using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaPalace.Migrations
{
    public partial class AddedNumberOfToppingsColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfToppings",
                table: "Order",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfToppings",
                table: "Order");
        }
    }
}
