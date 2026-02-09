using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiVendorECommerce.Migrations
{
    public partial class DeleteProductStatusFromProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductStatus",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductStatus",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
