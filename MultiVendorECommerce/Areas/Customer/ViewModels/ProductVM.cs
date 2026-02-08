using PermissionBasedAuz.Models;
using PermissionBasedAuz.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace PermissionBasedAuz.Areas.Customer.ViewModels
{
    public class ProductVM
    {
        [Required]
        public int Id { get; set; }
        [Required]

        public int VendorId { get; set; }
        [Required]

        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }=null!;

        [Required]

        public string Name { get; set; }=null!;
        [Required]
        public string Description { get; set; }=null!;
        [Required]

        public decimal Price { get; set; }
        [Required]

        public int StockQuantity { get; set; }
        [Required]

        public string ProductStatus { get; set; }=null!;

        public ICollection<string> Images { get; set; } = new List<string>();

        [Range(1, 90)]
        public decimal? DiscountPercentage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


    }
}
