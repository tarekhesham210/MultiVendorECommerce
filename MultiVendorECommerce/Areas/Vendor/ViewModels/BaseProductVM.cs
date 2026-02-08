using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PermissionBasedAuz.Areas.Vendor.ViewModels
{
    public class BaseProductVM:SelectCategoryVM
    {
        public int? Id { get; set; }

        [Required, MaxLength(100), MinLength(3)]
        public string Name { get; set; }= null!;

        [Required, MaxLength(500), MinLength(10)]
        public string Description { get; set; } = null!;

     

    }
}
