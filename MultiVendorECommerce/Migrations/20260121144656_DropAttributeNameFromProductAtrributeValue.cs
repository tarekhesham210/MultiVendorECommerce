using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiVendorECommerce.Migrations
{
    public partial class DropAttributeNameFromProductAtrributeValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttributeName",
                table: "ProductAttributeValue");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttributeName",
                table: "ProductAttributeValue",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
