using Fleama.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleama.Data.Configurations
{
    internal class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
    {
        public void Configure(EntityTypeBuilder<BlogPost> builder)
        {
            builder.Property(x => x.Title)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(x => x.Content)
                   .IsRequired();

            builder.Property(x => x.ImageUrl)
                   .HasMaxLength(1000);

            builder.HasOne(b => b.User)
                   .WithMany()
                   .HasForeignKey(b => b.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

