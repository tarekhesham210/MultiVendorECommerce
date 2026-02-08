using Microsoft.EntityFrameworkCore;
using PermissionBasedAuz.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace PermissionBasedAuz.Areas.Vendor.ViewModels
{
    public class ProductVariantVM: IValidatableObject
    {
        public int? Id { get; set; }
        public string SKU { get; set; }

        [Required, Precision(8, 2)]
        public decimal Price { get; set; }
        public ProductStatus ProductStatus { get; set; } = ProductStatus.Available;


        [Required, Range(0, 1000)]
        public int StockQuantity { get; set; }

        public bool HasOffer { get; set; }

        [Range(1, 90)]
        public decimal? DiscountPercentage { get; set; }
        
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public int? SelectedImageId { get; set; }
        public List<int> SelectedOptionIds { get; set; } = new();
        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            if (HasOffer)
            {
                if (DiscountPercentage == null)
                    yield return new ValidationResult("Discount is required",
                        new[] { nameof(DiscountPercentage) });

                if (StartDate == null || EndDate == null)
                    yield return new ValidationResult("Offer dates are required");
            }
        }
    }
}
