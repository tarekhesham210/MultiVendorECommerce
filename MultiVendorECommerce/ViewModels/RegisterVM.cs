using MultiVendorECommerce.Attributes;
using MultiVendorECommerce.Constants;
using MultiVendorECommerce.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace MultiVendorECommerce.ViewModels
{
    public class RegisterVM
    {


        [Required(ErrorMessage = "Email Adress Is Required")]
        [EmailAddress]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{6,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; } = null!;


        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        public string ConfirmPassword { get; set; } = null!;

        [ MaxFileSizeAttribute(ImageSettings.MaxFileSizeInBytes)]
        [AllowedExtensions(ImageSettings.AllowedExtensions)]

        public IFormFile? Image { get; set; } 

        [Required]
        public UserType userType { get; set; }


    }

}
