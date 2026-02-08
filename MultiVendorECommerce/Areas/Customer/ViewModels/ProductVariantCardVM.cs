namespace PermissionBasedAuz.Areas.Customer.ViewModels
{
    public class ProductVariantCardVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string MainImageUrl { get; set; }=null!;

        public int Discount { get; set; }
        public bool HasOffer { get; set; }
        public decimal FinalPrice  =>
            HasOffer && Discount > 0  ?
            Price * (100-Discount)/100 : Price;

    }
}
