using Microsoft.AspNetCore.Mvc.Rendering;
using PermissionBasedAuz.Models;
using PermissionBasedAuz.Shared.Enums;

namespace PermissionBasedAuz.Areas.Vendor.ViewModels
{
    public class CategoryAttributeVM
    {

        public int Id { get; set; }

        public string? Name { get; set; }           
        public bool IsRequired { get; set; }
        public bool IsVariant { get; set; }
        public int SelectedOptionId { get; set; }
        public string? SelectedOptionName { get; set; }
        public bool IsLocked { get; set; }
        public List<CategoryAttributeOptionVM> Options { get; set; } = new List<CategoryAttributeOptionVM>();

       


    }
}
