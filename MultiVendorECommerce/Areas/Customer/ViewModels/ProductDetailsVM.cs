
using PermissionBasedAuz.Shared.Enums;

namespace PermissionBasedAuz.Areas.Customer.ViewModels
{
    public class ProductDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        // الصور الأساسية للمنتج
        public List<string> GalleryImages { get; set; } = new();

        // النسخ المتاحة (Variants)
        public List<VariantDetailsVM> Variants { get; set; } = new();

        // 1. للـ Selectors (الزراير اللي العميل هيضغط عليها)
        // مثال: [Color: Red, Blue], [Size: XL, L]
        public List<ProductAttributeVM> SelectionAttributes { get; set; } = new();

        // 2. للمواصفات الثابتة (جدول المواصفات التقنية)
        // مثال: [Material: Cotton], [Warranty: 1 Year]
        public List<FixedAttributeVM> SpecificationAttributes { get; set; } = new();

        public int? RequestedVariantId { get; set; }

        public List<int> InitialSelectedIds { get; set; } = new();

    }

}
