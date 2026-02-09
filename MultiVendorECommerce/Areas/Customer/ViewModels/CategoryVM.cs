using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace MultiVendorECommerce.Areas.Customer.ViewModels
{
    public class CategoryVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }= null!;
        public int ProductsCount { get; set; }


    }
}
