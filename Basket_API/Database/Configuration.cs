using Basket_API.Domains.BasketItems;
using Basket_API.Domains.Baskets;
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

            builder.HasKey(b => b.BasketId).IsClustered(false);

            builder.Property(b => b.ClusterId).ValueGeneratedOnAdd();

            builder.HasIndex(b => b.ClusterId).IsClustered().IsUnique();
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

            builder.HasKey(bi => bi.BasketItemId).IsClustered(false);

            builder.Property(bi => bi.ClusterId).ValueGeneratedOnAdd();

            builder.HasIndex(bi => bi.ClusterId).IsClustered().IsUnique();
        }
    }
}
