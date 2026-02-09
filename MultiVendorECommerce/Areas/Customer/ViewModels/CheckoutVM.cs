using MultiVendorECommerce.Models;
using MultiVendorECommerce.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace MultiVendorECommerce.Areas.Customer.ViewModels
{
    public class CheckoutVM
    {
        [Required] public string Address { get; set; }
        [Required] public string City { get; set; }
        [Required] public string Phone { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public List<CartItemDetailsVM> CartItems { get; set; } = new();
        public decimal SubTotal { get; set; }//=> CartItems.Sum(i => i.TotalPrice);
        public decimal Shipping => 10;
        public decimal TotalAmount  =>SubTotal+Shipping;
    }
}
