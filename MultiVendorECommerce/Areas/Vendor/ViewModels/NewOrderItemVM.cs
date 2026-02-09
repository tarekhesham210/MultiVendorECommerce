namespace MultiVendorECommerce.Areas.Vendor.ViewModels
{
    public class NewOrderItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int VariantId { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }

    }
}
