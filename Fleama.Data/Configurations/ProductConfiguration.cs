using Fleama.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleama.Data.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.HasMany(p => p.Images)
                   .WithOne() 
                   .HasForeignKey(i => i.ReferenceId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.ProductCode)
                   .HasMaxLength(50);
        }
    }
}