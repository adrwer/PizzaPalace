using Microsoft.EntityFrameworkCore.Migrations;

namespace PizzaPalace.Migrations
{
    public partial class AddedSubtotalandTaxColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Subtotal",
                table: "Order",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "Order",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "Order");
        }
    }
}
