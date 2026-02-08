namespace PermissionBasedAuz.Areas.Customer.ViewModels
{
    public class CartItemDetailsVM
    {
        public int VariantId { get; set; }
        public string VariantName { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public decimal TotalPrice => Price * Quantity;

    }
}
