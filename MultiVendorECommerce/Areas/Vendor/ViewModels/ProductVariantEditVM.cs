namespace PermissionBasedAuz.Areas.Vendor.ViewModels
{
    public class ProductVariantEditVM:ProductVariantVM
    {
        public int Id { get; set; }
        public List<string> OptionNames { get; set; } = new();
    }
}
