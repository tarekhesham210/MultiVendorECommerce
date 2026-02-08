using System.ComponentModel.DataAnnotations;

namespace PermissionBasedAuz.ViewModels
{
    public class RoleVM
    {
        [Required,MaxLength(50)]
        public string Name { get; set; } = null!;
    }
}
