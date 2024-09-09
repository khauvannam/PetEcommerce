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
        public void Configure(EntityTypeBuilder<OrderModel> builder)
        {
            builder.HasKey(o => o.OrderId);
            builder
                .HasMany<OrderLine>()
                .WithOne(ol => ol.OrderModel)
                .HasForeignKey(ol => ol.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public static OrderConfigure Create()
        {
            return new OrderConfigure();
        }
    }

    public class OrderLineConfigure : IEntityTypeConfiguration<OrderLine>
    {
        public void Configure(EntityTypeBuilder<OrderLine> builder)
        {
            builder.HasKey(ol => ol.OrderLineId);
        }

        public static OrderConfigure Create()
        {
            return new OrderConfigure();
        }
    }

    public class ShippingMethodConfigure : IEntityTypeConfiguration<ShippingMethod>
    {
        public void Configure(EntityTypeBuilder<ShippingMethod> builder)
        {
            builder.HasKey(s => s.ShippingMethodId);
        }

        public static ShippingMethodConfigure Create()
        {
            return new ShippingMethodConfigure();
        }
    }
}
