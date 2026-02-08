namespace PermissionBasedAuz.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public decimal DiscountPercentage { get; set; } // 0 - 90
        public decimal? MaxDiscountAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();

        public bool IsActive =>
              StartDate<= DateTime.UtcNow   &&  EndDate>= DateTime.UtcNow;

    }
}
