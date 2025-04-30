using Fleama.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fleama.Data
{
    internal class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(x=>x.Name)
                                      .IsRequired()
                                      .HasColumnType("varchar(50)")
                                      .HasMaxLength(50);
            builder.Property(x=>x.UserName).IsRequired().HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(x=>x.Surname).IsRequired().HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(x=>x.Email).IsRequired().HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(x=>x.Phone).HasColumnType("varchar(15)").HasMaxLength(15);
            builder.Property(x=>x.Phone).IsRequired().HasColumnType("nvarchar(50)").HasMaxLength(50);
        }
    }
}