using Microsoft.EntityFrameworkCore;
using Order.API.Domains.OrderLines;
using Order.API.Domains.ShippingMethods;

namespace Order.API.Databases;

public class OrderDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Domains.Orders.Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }

    public DbSet<ShippingMethod> ShippingMethods { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(Configuration.OrderConfigure.Create());
        modelBuilder.ApplyConfiguration(Configuration.OrderLineConfigure.Create());
        modelBuilder.ApplyConfiguration(Configuration.ShippingMethodConfigure.Create());
    }
}
