using Microsoft.EntityFrameworkCore;
using Order.API.Domains.OrderLines;
using Order.API.Domains.Orders;
using Order.API.Domains.ShippingMethods;

namespace Order.API.Databases;

public class OrderDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<OrderModel> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }
    public DbSet<Shipping> ShippingMethods { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(Configuration.OrderConfigure.Create());
        modelBuilder.ApplyConfiguration(Configuration.OrderLineConfigure.Create());
        modelBuilder.ApplyConfiguration(Configuration.ShippingMethodConfigure.Create());
    }
}
