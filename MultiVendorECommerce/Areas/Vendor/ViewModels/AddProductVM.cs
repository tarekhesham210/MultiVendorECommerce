using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PermissionBasedAuz.Attribute;
using PermissionBasedAuz.Constants;
using PermissionBasedAuz.Models;
using PermissionBasedAuz.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace PermissionBasedAuz.Areas.Vendor.ViewModels
{
    public class AddProductVM : BaseProductVM
    {

        [Required, MaxFileSize(ImageSettings.MaxFileSizeInMB),
        AllowedExtensions(ImageSettings.AllowedExtensions)]
        [MinFilesCount(1)]
        public List<IFormFile> ProductImages { get; set; } = new List<IFormFile>();

        //Get
        public List<CategoryAttributeVM> AvailableAttributes { get; set; } = new List<CategoryAttributeVM>();

        //post
        public List<ProductAttributeValueVM> FixedAttributesValues { get; set; } = new List<ProductAttributeValueVM>();

        //post
        public List<ProductVariantVM> CreatedVariants { get; set; }= new List<ProductVariantVM>();

    }
}
