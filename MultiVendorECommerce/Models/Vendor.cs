using MultiVendorECommerce.Shared.Enums;

namespace MultiVendorECommerce.Models
{
    public class Vendor
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string StoreName { get; set; }
        public string StoreDescription { get; set; }

        public VendorStatus VendorStatus { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Product> Products { get; set; }
    }

}
