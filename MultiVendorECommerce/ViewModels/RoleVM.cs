using System.ComponentModel.DataAnnotations;

namespace MultiVendorECommerce.ViewModels
{
    public class RoleVM
    {
        [Required,MaxLength(50)]
        public string Name { get; set; } = null!;
    }
}
