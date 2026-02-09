namespace MultiVendorECommerce.Areas.Admin.ViewModels
{
    public class CategoryWithAttributeVM:CategoryVM
    {
        public List<CategoryAttributeVM> Attributes { get; set; } = new();

    }
}
