using Microsoft.AspNetCore.Mvc.Rendering;
using PermissionBasedAuz.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace PermissionBasedAuz.Areas.Admin.ViewModels
{
    public class NewRoleVM
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        public UserType RoleType { get; set; }

        public IEnumerable<SelectListItem> Types { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
