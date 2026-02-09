namespace MultiVendorECommerce.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductVariantId { get; set; }
        public ProductVariant Variant { get; set; }

        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool VendorConfirmation { get; set; }
    }

}
