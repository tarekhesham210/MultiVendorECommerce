using MultiVendorECommerce.Areas.Vendor.ViewModels;
using MultiVendorECommerce.Models;

namespace MultiVendorECommerce.Areas.Customer.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<CategoryVM> Categories { get; set; }=Enumerable.Empty<CategoryVM>();
        public IEnumerable<ProductVariantCardVM> BestSellers { get; set; }=Enumerable.Empty<ProductVariantCardVM>();
      //  public IEnumerable<Product> TrandyProducts { get; set; }=Enumerable.Empty<Product>();
        public IEnumerable<ProductWithHotOffer> HotOffers { get; set; }=Enumerable.Empty<ProductWithHotOffer>();
    }
}
