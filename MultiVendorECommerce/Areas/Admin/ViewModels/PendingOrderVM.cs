namespace MultiVendorECommerce.Areas.Admin.ViewModels
{
    public class PendingOrderVM
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingCity { get; set; }

        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string CustomerPhone { get; set; }



    }
}
