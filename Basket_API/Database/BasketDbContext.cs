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
        modelBuilder.ApplyConfiguration(new Configuration.BasketConfigure());

        modelBuilder.ApplyConfiguration(new Configuration.BasketItemConfigure());
    }
}
