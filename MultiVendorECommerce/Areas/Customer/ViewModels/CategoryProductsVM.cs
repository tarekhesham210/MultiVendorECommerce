using PermissionBasedAuz.Models;

namespace PermissionBasedAuz.Areas.Customer.ViewModels
{
    public class CategoryProductsVM
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public IEnumerable<ProductVariantCardVM> Products { get; set; }=Enumerable.Empty<ProductVariantCardVM>();
    }
}
