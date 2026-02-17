using MultiVendorECommerce.Models;

namespace MultiVendorECommerce.Areas.Customer.ViewModels
{
   
    public class SearchResultVM
    {
        public List<ProductVariantCardVM> Products { get; set; }=new List<ProductVariantCardVM>();
        public string CurrentSearchTerm { get; set; }
        public int? SelectedCategoryId { get; set; }
        public string CurrentSortBy { get; set; }

        // Pagination
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
