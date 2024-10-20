using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.API.Domain.OrderLines;
using Order.API.Domain.Orders;
using Order.API.Domain.Shippings;

namespace Order.API.Databases;

public static class Configuration
{
    public class OrderConfigure : IEntityTypeConfiguration<OrderModel>
    {
        public void Configure(EntityTypeBuilder<OrderModel> builder)
        {
            builder.HasKey(o => o.OrderId).IsClustered();

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
            builder.HasKey(ol => ol.OrderLineId).IsClustered();
        }

        public static OrderConfigure Create()
        {
            return new OrderConfigure();
        }
    }

    public class ShippingMethodConfigure : IEntityTypeConfiguration<Shipping>
    {
        public void Configure(EntityTypeBuilder<Shipping> builder)
        {
            builder.HasKey(s => s.ShippingId);
        }

        public static ShippingMethodConfigure Create()
        {
            return new ShippingMethodConfigure();
        }
    }
}
