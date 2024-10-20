using Identity.API.Domains.Tokens;
using Identity.API.Domains.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.API.Databases;

public static class Configuration
{
    public class UserConfigure : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne(u => u.RefreshToken)
                .WithOne(rt => rt.User)
                .HasForeignKey<RefreshToken>(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ComplexProperty(
                e => e.Address,
                addressBuilder =>
                {
                    addressBuilder
                        .Property(a => a.Street)
                        .HasColumnName("Street")
                        .HasMaxLength(250);

                    addressBuilder.Property(a => a.City).HasColumnName("City").HasMaxLength(250);

                    addressBuilder
                        .Property(a => a.ZipCode)
                        .HasColumnName("ZipCode")
                        .HasMaxLength(250);
                }
            );
        }
    }
}
