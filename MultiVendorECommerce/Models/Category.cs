namespace MultiVendorECommerce.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int? ParentCategoryId { get; set; }

        public string? ImageUrl { get; set; }

        public Category? ParentCategory { get; set; } = default!;

        public ICollection<Category> SubCategories { get; set; }=new List<Category>();
        public ICollection<Product> Products { get; set; } = new List<Product>();

        public ICollection<CategoryAttribute> Attributes { get; set; } = new List<CategoryAttribute>();

    }

}
