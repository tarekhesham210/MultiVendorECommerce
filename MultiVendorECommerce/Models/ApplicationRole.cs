using Microsoft.AspNetCore.Identity;
using PermissionBasedAuz.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace PermissionBasedAuz.Models
{
    public class ApplicationRole:IdentityRole
    {
        [Required]
        public UserType UserType { get; set; }

    }
}
