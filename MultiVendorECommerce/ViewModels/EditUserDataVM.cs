using MultiVendorECommerce.Attributes;
using MultiVendorECommerce.Constants;
using System.ComponentModel.DataAnnotations;

namespace MultiVendorECommerce.ViewModels
{
    public class EditUserDataVM
    {
        [Required]
        public string Id { get; set; }=null!;

        [Required(ErrorMessage = "Email Adress Is Required")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string? currentImage { get; set; }

        [MaxFileSizeAttribute(ImageSettings.MaxFileSizeInBytes)]
        [AllowedExtensions(ImageSettings.AllowedExtensions)]
        public IFormFile? Image { get; set; } 
    }
}
