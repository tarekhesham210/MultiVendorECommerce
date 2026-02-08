namespace PermissionBasedAuz.Areas.Vendor.ViewModels
{
    public class ProductAttributeValueVM
    {
        public int AttributeId { get; set; }
        public string? AttributeName { get; set; }
        public int SelectedOptionId { get; set; }

        public List<CategoryAttributeOptionVM> Options = new();
    }
}