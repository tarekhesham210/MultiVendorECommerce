using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiVendorECommerce.Models;

namespace MultiVendorECommerce.Configuration
{
    public class CartItemConfig : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.PriceAtTime)
                   .HasColumnType("decimal(18,2)");

            builder.HasIndex(x => new { x.CartId, x.ProductVariantId })
                   .IsUnique();
            builder.HasOne(x => x.Variant)
              .WithMany()
              .HasForeignKey(x => x.ProductVariantId)
              .OnDelete(DeleteBehavior.Restrict); 
        }
    }

}
