namespace PermissionBasedAuz.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<CartItem> Items { get; set; }
    }

}
