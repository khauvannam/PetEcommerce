using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product_API.Domains.Categories;
using Product_API.Domains.Products;

namespace Product_API.Databases;

public static class Configuration
{
    public class ProductConfigure : IEntityTypeConfiguration<Product>
    {
        public static ProductConfigure Create() => new();

        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.ProductId);

            builder
                .HasMany(p => p.ProductVariants)
                .WithOne(pv => pv.Product)
                .HasForeignKey(pv => pv.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ProductVariantConfigure : IEntityTypeConfiguration<ProductVariant>
    {
        public static ProductVariantConfigure Create() => new();

        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.HasKey(pv => pv.VariantId);

            builder.ComplexProperty(pv => pv.OriginalPrice);
            builder
                .Property(pv => pv.Discount)
                .HasConversion(d => d.Percent, arg => Discount.Create(arg));
        }
    }

    public class CategoryConfigure : IEntityTypeConfiguration<Category>
    {
        public static CategoryConfigure Create() => new();

        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.CategoryId);

            builder
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
