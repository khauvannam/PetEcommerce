using Microsoft.EntityFrameworkCore;
using Order.API.Domain.OrderLines;
using Order.API.Domain.Orders;
using Order.API.Domain.Shippings;

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
