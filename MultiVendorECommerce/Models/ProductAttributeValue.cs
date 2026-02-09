using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiVendorECommerce.Models
{
    //is variant=false

   // [Table("ProductAttributeValue")]
    public class ProductAttributeValue
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int CategoryAttributeId { get; set; }
        public CategoryAttribute CategoryAttribute { get; set; }

        public int CategoryAttributeOptionId { get; set; }
        public CategoryAttributeOption CategoryAttributeOption { get; set; }
    }
}
