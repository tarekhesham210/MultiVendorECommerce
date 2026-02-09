using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MultiVendorECommerce.Areas.Vendor.ViewModels
{   
    public class ManualVariantVM :IValidatableObject
    {
        public int? Id { get; set; } // Null for new ones
        public string SKU { get; set; }
        [Required, Precision(8, 2)]

        public decimal Price { get; set; }
        [Required, Range(0, 1000)]

        public int Stock { get; set; }
        public bool IsActive { get; set; } = true;

        public bool HasOffer { get; set; }

        [Range(1, 90)]
        public decimal? DiscountPercentage { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        // The key part: List of selected option IDs matching the active dimensions
        // e.g., [RedId, LargeId]
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