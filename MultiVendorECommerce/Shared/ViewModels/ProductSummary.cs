using PermissionBasedAuz.Models;
using PermissionBasedAuz.Shared.Enums;
using System.Collections;

namespace PermissionBasedAuz.Shared.ViewModels
{
    public class ProductSummary
    {
        public int Id { get; set; }
        public string Name { get; set; }=null!;
        public string Description { get; set; }=null!;

        public decimal Price { get; set; }
        public IEnumerable<string> Images { get; set; }=Enumerable.Empty<string>();
        public string CategoryName { get; set; } = null!;
        public string VendorStoreName { get; set; } = null!;
        public ProductStatus ProductStatus { get; set; }

        public Offer? Offer { get; set; }


    }
}
