using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiVendorECommerce.Models;

namespace MultiVendorECommerce.Configuration
{
    public class CategoryAttributeConfig : IEntityTypeConfiguration<CategoryAttribute>
    {
        public void Configure(EntityTypeBuilder<CategoryAttribute> builder)
        {
            builder.HasMany(c => c.Options)
                .WithOne(o => o.CategoryAttribute)
                .HasForeignKey(o => o.CategoryAttributeId);

            


           
        }
    }

}
