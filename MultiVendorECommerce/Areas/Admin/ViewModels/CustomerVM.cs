using MultiVendorECommerce.Shared.Enums;

namespace MultiVendorECommerce.Areas.Admin.ViewModels
{
    public class CustomerVM
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string FullName { get; set; } = null!;
    

        public string? Phone { get; set; }
        public string Email { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public CustomerStatus Status { get; set; }


    }
}
