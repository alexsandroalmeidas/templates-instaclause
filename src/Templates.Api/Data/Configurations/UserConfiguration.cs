using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Templates.Api.Data.Entities;

namespace Templates.Api.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.OwnsOne(u => u.Address, a =>
            {
                a.Property(p => p.Street).HasColumnName("Street").HasMaxLength(200);
                a.Property(p => p.HouseNumber).HasColumnName("HouseNumber").HasMaxLength(20);
                a.Property(p => p.City).HasColumnName("City").HasMaxLength(100);
                a.Property(p => p.State).HasColumnName("State").HasMaxLength(100);
                a.Property(p => p.Country).HasColumnName("Country").HasMaxLength(100);
                a.Property(p => p.ZipCode).HasColumnName("ZipCode").HasMaxLength(20);
            });

            builder.Property(u => u.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(p => p.PhoneNumber)
                   .HasMaxLength(20);
        }
    }
}
