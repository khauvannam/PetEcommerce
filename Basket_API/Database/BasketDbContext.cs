using Basket_API.Domain.BasketItems;
using Basket_API.Domain.Baskets;
using Microsoft.EntityFrameworkCore;

namespace Basket_API.Database;

public class BasketDbContext(DbContextOptions<BasketDbContext> options) : DbContext(options)
{
    public DbSet<BasketItem> BasketItems { get; init; }
    public DbSet<Basket> Baskets { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Basket>().HasKey(b => b.BasketId);

        modelBuilder
            .Entity<Basket>()
            .HasMany<BasketItem>()
            .WithOne(bi => bi.Basket)
            .HasForeignKey(bi => bi.BasketId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
