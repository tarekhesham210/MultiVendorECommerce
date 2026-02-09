namespace MultiVendorECommerce.Areas.Customer.ViewModels
{
    public class CategoriesHeaderVM
    {
        public IEnumerable<CategoriesForHomeVM> Categories { get; set; }=new List<CategoriesForHomeVM>();
    }
}
