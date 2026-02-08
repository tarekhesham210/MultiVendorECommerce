namespace PermissionBasedAuz.Areas.Customer.ViewModels
{
    public class ProductWithHotOffer
    {
        public int Id { get; set; }

        public string Name { get; set; }= null!;
        public int ProductId { get; set; }
        public string MainImageUrl { get; set; } = null!;

        public int Discount { get; set; }

        public decimal OldPrice { get; set; }

        public decimal NewPrice { get; set; }


    }
}
