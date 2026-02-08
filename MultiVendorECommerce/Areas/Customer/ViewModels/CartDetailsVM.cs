namespace PermissionBasedAuz.Areas.Customer.ViewModels
{
    public class CartDetailsVM
    {
        public decimal SubTotal => cartItems.Sum(i => i.TotalPrice);
        public decimal Shipping => 10;

        public List<CartItemDetailsVM> cartItems = new List<CartItemDetailsVM>();

    }
}
