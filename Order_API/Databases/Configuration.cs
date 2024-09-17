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
            builder.HasKey(o => o.OrderId).IsClustered(false);

            builder
                .HasMany<OrderLine>()
                .WithOne(ol => ol.OrderModel)
                .HasForeignKey(ol => ol.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(o => o.ClusterId).ValueGeneratedOnAdd();

            builder.HasIndex(o => o.ClusterId).IsClustered().IsUnique();
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
            builder.HasKey(ol => ol.OrderLineId).IsClustered(false);

            builder.Property(ol => ol.ClusterId).ValueGeneratedOnAdd();

            builder.HasIndex(ol => ol.ClusterId).IsClustered().IsUnique();
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
