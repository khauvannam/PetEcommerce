using Identity.API.Domain.Tokens;
using Identity.API.Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Databases;

public class UserDbContext(DbContextOptions<UserDbContext> options)
    : IdentityDbContext<User>(options)
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<User>()
            .HasOne(u => u.RefreshToken)
            .WithOne(rt => rt.User)
            .HasForeignKey<RefreshToken>(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<User>()
            .ComplexProperty(
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
