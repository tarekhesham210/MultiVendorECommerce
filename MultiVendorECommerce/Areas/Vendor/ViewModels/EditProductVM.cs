using Microsoft.EntityFrameworkCore;
using MultiVendorECommerce.Attributes;
using MultiVendorECommerce.Constants;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace MultiVendorECommerce.Areas.Vendor.ViewModels
{
    public class EditProductVM: BaseProductVM
    {
       
        
        public ProductStatus ProductStatus { get; set; }
        [MaxFileSize(ImageSettings.MaxFileSizeInMB),
        AllowedExtensions(ImageSettings.AllowedExtensions)]
        
        public List<IFormFile>? NewImages { get; set; } = new List<IFormFile>();

        public List<ProductImageVM> ExistingImages { get; set; } = new();

        // Schema: All attributes available for this category
        public List<ProductAttributeSchemaVM> Dimensions { get; set; } = new();

        public List<ProductAttributeValueVM> FixedAttributesValues { get; set; } = new();

        // Data: The actual rows in the table
        public List<ProductVariantVM> Variants { get; set; } = new();
        //get
        //   public List<CategoryAttributeVM> AvailableAttributes { get; set; } = new List<CategoryAttributeVM>();
        //   public List<ProductVariantEditVM> ExistingVariants { get; set; } = new List<ProductVariantEditVM>();

        //post
        // public List<ProductVariantVM> NewVariantts { get; set; } = new List<ProductVariantVM>();

    }
}
