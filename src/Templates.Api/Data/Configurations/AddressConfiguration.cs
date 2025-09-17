using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Templates.Api.Data.Entities;

namespace Templates.Api.Data.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Street)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(a => a.HouseNumber)
                   .HasMaxLength(20);

            builder.Property(a => a.City)
                   .HasMaxLength(100);

            builder.Property(a => a.State)
                   .HasMaxLength(100);

            builder.Property(a => a.Country)
                   .HasMaxLength(100);

            builder.Property(a => a.ZipCode)
                   .HasMaxLength(20);
        }
    }
}
