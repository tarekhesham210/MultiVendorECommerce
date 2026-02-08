using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PermissionBasedAuz.Models
{
    //[Table("CategoryAttributeOption")]
    public class CategoryAttributeOption
    {
        public int Id { get; set; }

        public int CategoryAttributeId { get; set; }
        public CategoryAttribute CategoryAttribute { get; set; }

        public string Value { get; set; } //red,blue
    }

}
