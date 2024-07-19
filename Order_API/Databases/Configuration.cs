using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.API.Domains.OrderLines;
using Order.API.Domains.Orders;
using Order.API.Domains.ShippingMethods;

namespace Order.API.Databases;

public static class Configuration
{
    public class OrderConfigure : IEntityTypeConfiguration<OrderModel>
    {
        public static OrderConfigure Create() => new();

        public void Configure(EntityTypeBuilder<OrderModel> builder)
        {
            builder.HasKey(o => o.OrderId);
            builder
                .HasMany<OrderLine>()
                .WithOne(ol => ol.OrderModel)
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
        }
    }
}
