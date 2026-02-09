using MultiVendorECommerce.Shared.Enums;

namespace MultiVendorECommerce.Areas.Customer.ViewModels
{
    public class ProductAttributeVM
    {
        public int AttributeId { get; set; }

        public string Name { get; set; }           

        public List<ProductAttributeOptionVM> Values { get; set; } = new();
    }

}
