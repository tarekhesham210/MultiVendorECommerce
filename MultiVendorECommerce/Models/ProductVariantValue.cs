namespace PermissionBasedAuz.Models
{
    //is variant=true
    public class ProductVariantValue
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; }

        public int CategoryAttributeId { get; set; }
        public CategoryAttribute CategoryAttribute { get; set; }
        public int CategoryAttributeOptionId { get; set; }
        public CategoryAttributeOption CategoryAttributeOption { get; set; }
    }
}