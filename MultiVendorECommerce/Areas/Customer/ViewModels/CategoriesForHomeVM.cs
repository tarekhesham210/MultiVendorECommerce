using Microsoft.AspNetCore.Mvc.Rendering;

namespace PermissionBasedAuz.Areas.Customer.ViewModels
{
    public class CategoriesForHomeVM
    {
        public int Id { get; set; }
        public string Name { get; set; }=null!;

        public IEnumerable<SelectListItem> SubCategories { get; set; }=new List<SelectListItem>();
    }
}
