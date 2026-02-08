using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PermissionBasedAuz.Migrations
{
    public partial class AddVariantImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VariantImageId",
                table: "ProductVariants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_VariantImageId",
                table: "ProductVariants",
                column: "VariantImageId",
                unique: true,
                filter: "[VariantImageId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariants_ProductImages_VariantImageId",
                table: "ProductVariants",
                column: "VariantImageId",
                principalTable: "ProductImages",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariants_ProductImages_VariantImageId",
                table: "ProductVariants");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariants_VariantImageId",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "VariantImageId",
                table: "ProductVariants");
        }
    }
}
