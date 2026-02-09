using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiVendorECommerce.Migrations
{
    public partial class AddSoldCountToProductVariant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SoldCount",
                table: "Products",
                newName: "TotalSoldCount");

            migrationBuilder.AddColumn<int>(
                name: "SoldCount",
                table: "ProductVariants",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoldCount",
                table: "ProductVariants");

            migrationBuilder.RenameColumn(
                name: "TotalSoldCount",
                table: "Products",
                newName: "SoldCount");
        }
    }
}
