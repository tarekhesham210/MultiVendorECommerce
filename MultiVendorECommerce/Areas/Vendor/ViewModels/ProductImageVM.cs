namespace MultiVendorECommerce.Areas.Vendor.ViewModels
{
    public class ProductImageVM
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }

        
        public bool MarkForDelete { get; set; }
        public IFormFile? NewImage { get; set; }
    }
}
