using MultiVendorECommerce.Exceptions;
using MultiVendorECommerce.Shared.Enums;

namespace MultiVendorECommerce.Models
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get;private set; }
        public ProductStatus ProductStatus { get; private set; } = ProductStatus.Available;
        public int? VariantImageId { get; set; }
        public ProductImage Image { get; set; }
        public ICollection<ProductVariantValue> VariantValues { get; set; } = new List<ProductVariantValue>();
         public int? OfferId { get; set; }
         public Offer? CurrentOffer { get; set; }
        public int SoldCount { get; set; }
         public decimal FinalPrice =>
             CurrentOffer !=null && CurrentOffer.IsActive
                 ? Price *  (100 -CurrentOffer.DiscountPercentage)/ 100
                 : Price;
        public void SetStockQuantity(int quantity)
        {
            if (quantity < 0)
                throw new BusinessRuleException("Stock quantity cannot be negative.");

            StockQuantity = quantity;
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            ProductStatus = StockQuantity < 1
                ? ProductStatus.OutOfStock
                : ProductStatus.Available;
        }
    }
}
