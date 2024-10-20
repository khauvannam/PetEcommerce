using Basket_API.Domain.BasketItems;
using Basket_API.Domain.Baskets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basket_API.Database;

public static class Configuration
{
    public class BasketConfigure : IEntityTypeConfiguration<Basket>
    {
        public void Configure(EntityTypeBuilder<Basket> builder)
        {
            builder
                .HasMany<BasketItem>(bi => bi.BasketItemsList)
                .WithOne(bi => bi.Basket)
                .HasForeignKey(bi => bi.BasketId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasKey(b => b.BasketId).IsClustered();
        }
    }

    public class BasketItemConfigure : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder
                .Property(bi => bi.Quantity)
                .HasConversion(value => value.Value, value => Quantity.Create(value))
                .HasColumnName("Quantity");

            builder.HasKey(bi => bi.BasketItemId).IsClustered();
        }
    }
}
