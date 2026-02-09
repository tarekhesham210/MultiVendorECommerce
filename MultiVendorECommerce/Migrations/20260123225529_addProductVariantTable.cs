using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiVendorECommerce.Migrations
{
    public partial class addProductVariantTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryAttribute_Categories_CategoryId",
                table: "CategoryAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryAttributeOption_CategoryAttribute_CategoryAttributeId",
                table: "CategoryAttributeOption");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeValue_CategoryAttribute_CategoryAttributeId",
                table: "ProductAttributeValue");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeValue_Products_ProductId",
                table: "ProductAttributeValue");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Offers_OfferId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_OfferId",
                table: "Products");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_Price_NonNegative",
                table: "Products");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_StockQuantity_NonNegative",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductAttributeValue",
                table: "ProductAttributeValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryAttributeOption",
                table: "CategoryAttributeOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryAttribute",
                table: "CategoryAttribute");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StockQuantity",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "CategoryAttribute");

            migrationBuilder.RenameTable(
                name: "ProductAttributeValue",
                newName: "ProductAttributeValues");

            migrationBuilder.RenameTable(
                name: "CategoryAttributeOption",
                newName: "CategoryAttributeOptions");

            migrationBuilder.RenameTable(
                name: "CategoryAttribute",
                newName: "CategoryAttributes");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "OrderItems",
                newName: "ProductVariantId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                newName: "IX_OrderItems_ProductVariantId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "CartItems",
                newName: "ProductVariantId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                newName: "IX_CartItems_ProductVariantId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_CartId_ProductId",
                table: "CartItems",
                newName: "IX_CartItems_CartId_ProductVariantId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAttributeValue_ProductId",
                table: "ProductAttributeValues",
                newName: "IX_ProductAttributeValues_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAttributeValue_CategoryAttributeId",
                table: "ProductAttributeValues",
                newName: "IX_ProductAttributeValues_CategoryAttributeId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryAttributeOption_CategoryAttributeId",
                table: "CategoryAttributeOptions",
                newName: "IX_CategoryAttributeOptions_CategoryAttributeId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryAttribute_CategoryId",
                table: "CategoryAttributes",
                newName: "IX_CategoryAttributes_CategoryId");

            migrationBuilder.AddColumn<decimal>(
                name: "MaxDiscountAmount",
                table: "Offers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVariant",
                table: "CategoryAttributes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductAttributeValues",
                table: "ProductAttributeValues",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryAttributeOptions",
                table: "CategoryAttributeOptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryAttributes",
                table: "CategoryAttributes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProductVariants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    ProductStatus = table.Column<int>(type: "int", nullable: false),
                    OfferId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariants", x => x.Id);
                    table.CheckConstraint("CK_Variant_Price_NonNegative", "[Price] >= 0");
                    table.CheckConstraint("CK_Variant_Stock_NonNegative", "[StockQuantity] >= 0");
                    table.ForeignKey(
                        name: "FK_ProductVariants_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ProductVariants_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariantValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductVariantId = table.Column<int>(type: "int", nullable: false),
                    CategoryAttributeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariantValues_CategoryAttributes_CategoryAttributeId",
                        column: x => x.CategoryAttributeId,
                        principalTable: "CategoryAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductVariantValues_ProductVariants_ProductVariantId",
                        column: x => x.ProductVariantId,
                        principalTable: "ProductVariants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_OfferId",
                table: "ProductVariants",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductVariants",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantValues_CategoryAttributeId",
                table: "ProductVariantValues",
                column: "CategoryAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantValues_ProductVariantId",
                table: "ProductVariantValues",
                column: "ProductVariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ProductVariants_ProductVariantId",
                table: "CartItems",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryAttributeOptions_CategoryAttributes_CategoryAttributeId",
                table: "CategoryAttributeOptions",
                column: "CategoryAttributeId",
                principalTable: "CategoryAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryAttributes_Categories_CategoryId",
                table: "CategoryAttributes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_ProductVariants_ProductVariantId",
                table: "OrderItems",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeValues_CategoryAttributes_CategoryAttributeId",
                table: "ProductAttributeValues",
                column: "CategoryAttributeId",
                principalTable: "CategoryAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeValues_Products_ProductId",
                table: "ProductAttributeValues",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ProductVariants_ProductVariantId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryAttributeOptions_CategoryAttributes_CategoryAttributeId",
                table: "CategoryAttributeOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryAttributes_Categories_CategoryId",
                table: "CategoryAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_ProductVariants_ProductVariantId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeValues_CategoryAttributes_CategoryAttributeId",
                table: "ProductAttributeValues");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeValues_Products_ProductId",
                table: "ProductAttributeValues");

            migrationBuilder.DropTable(
                name: "ProductVariantValues");

            migrationBuilder.DropTable(
                name: "ProductVariants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductAttributeValues",
                table: "ProductAttributeValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryAttributes",
                table: "CategoryAttributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryAttributeOptions",
                table: "CategoryAttributeOptions");

            migrationBuilder.DropColumn(
                name: "MaxDiscountAmount",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "IsVariant",
                table: "CategoryAttributes");

            migrationBuilder.RenameTable(
                name: "ProductAttributeValues",
                newName: "ProductAttributeValue");

            migrationBuilder.RenameTable(
                name: "CategoryAttributes",
                newName: "CategoryAttribute");

            migrationBuilder.RenameTable(
                name: "CategoryAttributeOptions",
                newName: "CategoryAttributeOption");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "OrderItems",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_ProductVariantId",
                table: "OrderItems",
                newName: "IX_OrderItems_ProductId");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "CartItems",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_ProductVariantId",
                table: "CartItems",
                newName: "IX_CartItems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_CartId_ProductVariantId",
                table: "CartItems",
                newName: "IX_CartItems_CartId_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAttributeValues_ProductId",
                table: "ProductAttributeValue",
                newName: "IX_ProductAttributeValue_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductAttributeValues_CategoryAttributeId",
                table: "ProductAttributeValue",
                newName: "IX_ProductAttributeValue_CategoryAttributeId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryAttributes_CategoryId",
                table: "CategoryAttribute",
                newName: "IX_CategoryAttribute_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryAttributeOptions_CategoryAttributeId",
                table: "CategoryAttributeOption",
                newName: "IX_CategoryAttributeOption_CategoryAttributeId");

            migrationBuilder.AddColumn<int>(
                name: "OfferId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "StockQuantity",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "CategoryAttribute",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductAttributeValue",
                table: "ProductAttributeValue",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryAttribute",
                table: "CategoryAttribute",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryAttributeOption",
                table: "CategoryAttributeOption",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OfferId",
                table: "Products",
                column: "OfferId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_Price_NonNegative",
                table: "Products",
                sql: "[Price] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_StockQuantity_NonNegative",
                table: "Products",
                sql: "[StockQuantity] >= 0");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryAttribute_Categories_CategoryId",
                table: "CategoryAttribute",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryAttributeOption_CategoryAttribute_CategoryAttributeId",
                table: "CategoryAttributeOption",
                column: "CategoryAttributeId",
                principalTable: "CategoryAttribute",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeValue_CategoryAttribute_CategoryAttributeId",
                table: "ProductAttributeValue",
                column: "CategoryAttributeId",
                principalTable: "CategoryAttribute",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeValue_Products_ProductId",
                table: "ProductAttributeValue",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Offers_OfferId",
                table: "Products",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
