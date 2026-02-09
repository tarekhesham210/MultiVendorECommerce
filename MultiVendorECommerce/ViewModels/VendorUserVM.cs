using MultiVendorECommerce.Shared.Enums;

namespace MultiVendorECommerce.ViewModels
{
    public class VendorUserVM
    {
        public int Id { get; set; }
        public string userId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string StoreName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public VendorStatus vendorStatus { get; set; }

    }
}
