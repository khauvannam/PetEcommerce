using Basket_API.Domain.BasketItems;
using Basket_API.Domain.Baskets;
using Microsoft.EntityFrameworkCore;

namespace Basket_API.Database;

public class BasketDbContext(DbContextOptions<BasketDbContext> options) : DbContext(options)
{
    public DbSet<Basket> Baskets { get; init; }
    public DbSet<BasketItem> BasketItems { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Basket>().HasKey(b => b.BasketId);

        modelBuilder
            .Entity<Basket>()
            .HasMany<BasketItem>(bi => bi.BasketItemsList)
            .WithOne(bi => bi.Basket)
            .HasForeignKey(bi => bi.BasketId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BasketItem>().HasKey(bi => bi.BasketItemId);

        modelBuilder
            .Entity<BasketItem>()
            .Property(bi => bi.Quantity)
            .HasConversion(value => value.Value, value => Quantity.Create(value))
            .HasColumnName("Quantity");

        modelBuilder.Entity<BasketItem>().Property(bi => bi.Price).HasColumnType("decimal(18,2)");
    }
}
