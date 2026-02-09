namespace MultiVendorECommerce.Areas.Admin.ViewModels
{
    public class CustomerDetailsVM
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string Status { get; set; }
        public string? Image { get; set; }

    }
}
