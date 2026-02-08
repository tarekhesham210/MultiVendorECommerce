using PermissionBasedAuz.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace PermissionBasedAuz.Models
{
   // [Table("CategoryAttribute")]
    public class CategoryAttribute
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string Name { get; set; }           // RAM, Color, Size
        public bool IsVariant { get; set; }
        public bool IsRequired { get; set; }

        // لو Select
        public ICollection<CategoryAttributeOption> Options { get; set; }=new List<CategoryAttributeOption>();
    }

}
