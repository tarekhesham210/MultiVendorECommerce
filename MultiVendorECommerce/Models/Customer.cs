using PermissionBasedAuz.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace PermissionBasedAuz.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Phone { get; set; }

        [Required]
        public CustomerStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public Cart Cart { get; set; }
        public ICollection<Order> Orders { get; set; }=new List<Order>();
    }

}
