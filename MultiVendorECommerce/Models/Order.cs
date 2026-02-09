using MultiVendorECommerce.Shared.Enums;

namespace MultiVendorECommerce.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public string ShippingAddress { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public ICollection<OrderItem> Items { get; set; }
    }

}
