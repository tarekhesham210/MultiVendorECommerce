using MultiVendorECommerce.Areas.Customer.ViewModels;

namespace MultiVendorECommerce.Areas.Admin.ViewModels
{
    public class OrderDetailsVM
    {
        public int Id { get; set; }

        public List<OrderItemDetailsVM> Products = new List<OrderItemDetailsVM>();
        public decimal Shipping = 10;
        public decimal GrandTotal { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public string Phone { get; set; }
    }
}
