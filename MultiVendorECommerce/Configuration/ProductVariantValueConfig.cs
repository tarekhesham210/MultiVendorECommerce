using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PermissionBasedAuz.Models;

namespace PermissionBasedAuz.Configuration
{
    public class ProductVariantValueConfig : IEntityTypeConfiguration<ProductVariantValue>
    {
        public void Configure(EntityTypeBuilder<ProductVariantValue> builder)
        {
            builder.HasOne(v => v.CategoryAttribute)
                .WithMany()
                .HasForeignKey(v => v.CategoryAttributeId);

            builder.HasOne(v => v.CategoryAttributeOption)
                .WithMany()
                .HasForeignKey(v => v.CategoryAttributeOptionId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    

    }
}
