namespace MultiVendorECommerce.Areas.Admin.ViewModels
{
    public class OrderItemDetailsVM
    {
        public int ItemID { get; set; }
        public int VendorId { get; set; }
        public int VariantId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public bool IsVendorConfirmed { get; set; }
    }
}
