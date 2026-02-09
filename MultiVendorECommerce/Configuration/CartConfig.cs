using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiVendorECommerce.Models;

namespace MultiVendorECommerce.Configuration
{
    public class CartConfig : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Customer)
                   .WithOne(c => c.Cart)
                   .HasForeignKey<Cart>(x => x.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Items)
                   .WithOne(i => i.Cart)
                   .HasForeignKey(i => i.CartId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
