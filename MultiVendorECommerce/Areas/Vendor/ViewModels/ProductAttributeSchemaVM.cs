namespace MultiVendorECommerce.Areas.Vendor.ViewModels
{
    public class ProductAttributeSchemaVM
    {
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public bool IsActive { get; set; } // Is this dimension used for this product?
        public List<CategoryAttributeOptionVM> AllowableOptions { get; set; } = new();
    }
}