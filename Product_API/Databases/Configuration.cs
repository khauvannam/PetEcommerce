using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product_API.Domain.Categories;
using Product_API.Domain.Comments;
using Product_API.Domain.Discounts;
using Product_API.Domain.Products;

namespace Product_API.Databases;

public static class Configuration
{
    public class ProductConfigure : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.ProductId);

            builder.HasIndex(p => p.Name).IsUnique();

            builder
                .HasMany(p => p.ProductVariants)
                .WithOne(pv => pv.Product)
                .HasForeignKey(pv => pv.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(p => p.Comments)
                .WithOne(c => c.Product)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Property(p => p.DiscountPercent)
                .HasConversion(d => d.Value, arg => DiscountPercent.Create(arg));

            builder.Property(p => p.TotalRating).HasPrecision(18, 2);

            builder.Property(e => e.ProductBuyerIds).IsUnicode();
        }

        public static ProductConfigure Create()
        {
            return new ProductConfigure();
        }
    }

    public class ProductVariantConfigure : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.HasKey(pv => pv.VariantId);

            builder.ComplexProperty(pv => pv.OriginalPrice);
        }

        public static ProductVariantConfigure Create()
        {
            return new ProductVariantConfigure();
        }
    }

    public class CategoryConfigure : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.CategoryId);

            builder.HasIndex(p => new { p.Endpoint, p.CategoryName }).IsUnique();
        }

        public static CategoryConfigure Create()
        {
            return new CategoryConfigure();
        }
    }

    public class DiscountConfigure : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.HasKey(d => d.DiscountId);

            builder.Property(d => d.Percent).HasColumnType("decimal(18, 2)");
        }

        public static DiscountConfigure Create()
        {
            return new DiscountConfigure();
        }
    }

    public class CommentConfigure : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.CommentId);
        }
    }
}
