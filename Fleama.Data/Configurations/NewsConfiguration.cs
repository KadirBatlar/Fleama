using Fleama.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleama.Data.Configurations
{
    internal class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.HasOne(c => c.Image)
                   .WithMany()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}