using Microsoft.AspNetCore.Identity;
using PermissionBasedAuz.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace PermissionBasedAuz.Models
{
    public class ApplicationUser:IdentityUser
    {
        [MaxLength(256)]
        public string? Image { get; set; } 

        [Required]
        public UserType UserType { get; set; }

        public Customer? Customer { get; set; }
        public Vendor? Vendor { get; set; }
    }
}
