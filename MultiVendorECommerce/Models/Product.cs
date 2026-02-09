using MultiVendorECommerce.Exceptions;
using MultiVendorECommerce.Shared.Enums;

namespace MultiVendorECommerce.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

       

       // public decimal Price { get; set; }

      //  public int StockQuantity { get;private set; }

        public DateTime CreatedAt { get; set; }=DateTime.UtcNow;

        public ICollection<ProductImage> Images { get; set; }=new List<ProductImage>();

        public ICollection<ProductVariant> Variants { get; set; }=new List<ProductVariant>();
        public ICollection<ProductAttributeValue> AttributeValues { get; set; }=new List<ProductAttributeValue>();

        public int TotalSoldCount { get; set; }
        public int ViewsCount { get; set; }

       // public int? OfferId { get; set; }
       // public Offer? CurrentOffer { get; set; }
       // public decimal FinalPrice =>
       //     CurrentOffer !=null && CurrentOffer.IsActive
       //         ? Price *  (100 -CurrentOffer.DiscountPercentage)/ 100
       //         : Price;

        //public void SetStockQuantity(int quantity)
        //{
        //    if (quantity < 0)
        //        throw new BusinessRuleException("Stock quantity cannot be negative.");

        //    StockQuantity = quantity;
        //    UpdateStatus();
        //}

        //private void UpdateStatus()
        //{
        //    ProductStatus = StockQuantity < 1
        //        ? ProductStatus.OutOfStock
        //        : ProductStatus.Available;
        //}

    }

}
