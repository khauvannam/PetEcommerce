using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.API.Domains.OrderLines;
using Order.API.Domains.ShippingMethods;

namespace Order.API.Databases;

public static class Configuration
{
    public class OrderConfigure : IEntityTypeConfiguration<Domains.Orders.Order>
    {
        public static OrderConfigure Create() => new();

        public void Configure(EntityTypeBuilder<Domains.Orders.Order> builder)
        {
            builder.HasKey(o => o.OrderId);
            builder
                .HasMany<OrderLine>()
                .WithOne(ol => ol.Order)
                .HasForeignKey(ol => ol.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class OrderLineConfigure : IEntityTypeConfiguration<OrderLine>
    {
        public static OrderConfigure Create() => new();

        public void Configure(EntityTypeBuilder<OrderLine> builder)
        {
            builder.HasKey(ol => ol.OrderLineId);
        }
    }

    public class ShippingMethodConfigure : IEntityTypeConfiguration<ShippingMethod>
    {
        public static ShippingMethodConfigure Create() => new();

        public void Configure(EntityTypeBuilder<ShippingMethod> builder)
        {
            builder.HasKey(s => s.ShippingMethodId);
            builder
                .HasMany<Domains.Orders.Order>()
                .WithOne(o => o.ShippingMethod)
                .HasForeignKey(o => o.ShippingMethodId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
