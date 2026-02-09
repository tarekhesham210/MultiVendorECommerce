using Microsoft.AspNetCore.Identity;
using MultiVendorECommerce.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace MultiVendorECommerce.Models
{
    public class ApplicationRole:IdentityRole
    {
        [Required]
        public UserType UserType { get; set; }

    }
}
