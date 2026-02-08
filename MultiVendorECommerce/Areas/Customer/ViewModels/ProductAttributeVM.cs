using PermissionBasedAuz.Shared.Enums;

namespace PermissionBasedAuz.Areas.Customer.ViewModels
{
    public class ProductAttributeVM
    {
        public int AttributeId { get; set; }

        public string Name { get; set; }           

        public List<ProductAttributeOptionVM> Values { get; set; } = new();
    }

}
