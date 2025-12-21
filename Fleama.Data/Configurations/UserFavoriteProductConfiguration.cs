using Fleama.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleama.Data.Configurations
{
    internal class UserFavoriteProductConfiguration : IEntityTypeConfiguration<UserFavoriteProduct>
    {
        public void Configure(EntityTypeBuilder<UserFavoriteProduct> builder)
        {
            // Configure the relationship between UserFavoriteProduct and AppUser
            builder.HasOne(ufp => ufp.User)
                   .WithMany(u => u.FavoriteProducts)
                   .HasForeignKey(ufp => ufp.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Configure the relationship between UserFavoriteProduct and Product
            builder.HasOne(ufp => ufp.Product)
                   .WithMany(p => p.FavoritedByUsers)
                   .HasForeignKey(ufp => ufp.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Add unique constraint to prevent duplicate favorites
            builder.HasIndex(ufp => new { ufp.UserId, ufp.ProductId })
                   .IsUnique()
                   .HasDatabaseName("IX_UserFavoriteProduct_UserId_ProductId");

            // Configure UserId as required
            builder.Property(ufp => ufp.UserId)
                   .IsRequired();

            // Configure ProductId as required
            builder.Property(ufp => ufp.ProductId)
                   .IsRequired();
        }
    }
}
