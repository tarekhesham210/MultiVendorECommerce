using MultiVendorECommerce.Models;

namespace MultiVendorECommerce.Areas.Vendor.ViewModels
{
    public class OfferVM
    {
        public int Id { get; set; }
        public decimal DiscountPercentage { get; set; } // 0 - 90
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
     
    }
}
