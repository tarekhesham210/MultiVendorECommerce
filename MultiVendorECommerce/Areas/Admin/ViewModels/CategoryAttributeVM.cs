using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using PermissionBasedAuz.Models;
using PermissionBasedAuz.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace PermissionBasedAuz.Areas.Admin.ViewModels
{
    public class CategoryAttributeVM
    {
        public int? Id { get; set; } // null في Add

        [Required]
        public string Name { get; set; }

        public bool IsRequired { get; set; }
        public bool IsVariant { get; set; }
        public bool IsDeleted { get; set; }

        public List<CategoryAttributeOptionVM> Options { get; set; } = new();

    

    }
}
