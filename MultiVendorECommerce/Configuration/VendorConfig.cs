using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PermissionBasedAuz.Models;

namespace PermissionBasedAuz.Configuration
{
    public class VendorConfig : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                   .WithOne(u => u.Vendor)
                   .HasForeignKey<Vendor>(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.StoreName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.StoreDescription).HasMaxLength(500);
            builder.Property(x => x.VendorStatus).IsRequired();

            builder.HasMany(x => x.Products)
              .WithOne(p => p.Vendor)
              .HasForeignKey(p => p.VendorId)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
