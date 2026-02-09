namespace MultiVendorECommerce.Areas.Customer.ViewModels
{
    public class VariantDetailsVM
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal? OfferPrice { get; set; } // السعر بعد الخصم
        public int Stock { get; set; }
        public string SKU { get; set; }
        public string ImageUrl { get; set; }
        public List<int> OptionIds { get; set; } = new(); // عشان نعرف النسخة دي تبع أنهي اختيارات
    }
}
