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

            builder.Property(u => u.CreatedAt)
                   .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(p => p.PhoneNumber)
                   .HasMaxLength(20);

            builder.HasOne(u => u.Address)
                   .WithOne(a => a.User)
                   .HasForeignKey<Address>(a => a.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
