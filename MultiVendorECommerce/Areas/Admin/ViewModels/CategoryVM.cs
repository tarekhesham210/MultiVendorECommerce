using Microsoft.AspNetCore.Mvc.Rendering;
using MultiVendorECommerce.Models;
using System.ComponentModel.DataAnnotations;

namespace MultiVendorECommerce.Areas.Admin.ViewModels
{
    public class CategoryVM
    {
        public int Id { get; set; }


        [Required,MaxLength(50),MinLength(3)]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string Description { get; set; }=string.Empty;

        [Display(Name ="Parent Category")]
        public int? ParentCategoryId { get; set; }
      
        public string? ParentCategoryName { get; set; }

        public IFormFile? NewImage { get; set; }


        public IEnumerable<SelectListItem> ParentCategories { get; set; }=Enumerable.Empty<SelectListItem>();

    }
}