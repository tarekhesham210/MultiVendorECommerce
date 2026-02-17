using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MultiVendorECommerce.Attributes;
using MultiVendorECommerce.Constants;
using MultiVendorECommerce.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace MultiVendorECommerce.ViewModels
{
    public class AddProductVM
    {
        [Required,MaxLength(100),MinLength(3)]
        public string Name { get; set; } = null!;
        [Required]
        [Display(Name ="Category")]
        public int CaegoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
        [Required, MaxLength(500), MinLength(10)]
        public string Description { get; set; }=null!;
        [Required,Precision(8,2)]
        public decimal Price { get; set; }

        [Required,Range(1,1000)]
        public int StockQuantity { get; set; }

        public ProductStatus ProductStatus { get; set; }=ProductStatus.Available;


        [Required,MaxFileSize(ImageSettings.MaxFileSizeInMB),
        AllowedExtensions(ImageSettings.AllowedExtensions)]
        [MinFilesCount(1)]
        public List<IFormFile> ProductImages { get; set; }=new List<IFormFile>();

    }
}
