using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PermissionBasedAuz.Migrations
{
    public partial class addoptionIdInVariantValueAndProductValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "ProductVariantValues");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "ProductAttributeValues");

            migrationBuilder.AddColumn<int>(
                name: "CategoryAttributeOptionId",
                table: "ProductVariantValues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryAttributeOptionId",
                table: "ProductAttributeValues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantValues_CategoryAttributeOptionId",
                table: "ProductVariantValues",
                column: "CategoryAttributeOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeValues_CategoryAttributeOptionId",
                table: "ProductAttributeValues",
                column: "CategoryAttributeOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeValues_CategoryAttributeOptions_CategoryAttributeOptionId",
                table: "ProductAttributeValues",
                column: "CategoryAttributeOptionId",
                principalTable: "CategoryAttributeOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantValues_CategoryAttributeOptions_CategoryAttributeOptionId",
                table: "ProductVariantValues",
                column: "CategoryAttributeOptionId",
                principalTable: "CategoryAttributeOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeValues_CategoryAttributeOptions_CategoryAttributeOptionId",
                table: "ProductAttributeValues");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantValues_CategoryAttributeOptions_CategoryAttributeOptionId",
                table: "ProductVariantValues");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariantValues_CategoryAttributeOptionId",
                table: "ProductVariantValues");

            migrationBuilder.DropIndex(
                name: "IX_ProductAttributeValues_CategoryAttributeOptionId",
                table: "ProductAttributeValues");

            migrationBuilder.DropColumn(
                name: "CategoryAttributeOptionId",
                table: "ProductVariantValues");

            migrationBuilder.DropColumn(
                name: "CategoryAttributeOptionId",
                table: "ProductAttributeValues");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "ProductVariantValues",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "ProductAttributeValues",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
