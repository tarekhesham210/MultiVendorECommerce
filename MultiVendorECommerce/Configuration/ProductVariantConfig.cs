using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PermissionBasedAuz.Models;

namespace PermissionBasedAuz.Configuration
{
    public class ProductVariantConfig: IEntityTypeConfiguration<ProductVariant>
    {       
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
            builder.Property(x => x.SKU).IsRequired().HasMaxLength(100);

            builder.HasCheckConstraint("CK_Variant_Price_NonNegative", "[Price] >= 0");
            builder.HasCheckConstraint("CK_Variant_Stock_NonNegative", "[StockQuantity] >= 0");

            builder.HasOne(v => v.CurrentOffer)
                   .WithMany(o => o.Variants)
                   .HasForeignKey(v => v.OfferId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(v => v.Image)
                   .WithOne(i => i.Variant)
                   .HasForeignKey<ProductVariant>(v => v.VariantImageId)
                   .IsRequired(false);


        }
    }
}
