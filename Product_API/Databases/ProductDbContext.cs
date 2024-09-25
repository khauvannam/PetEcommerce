using Microsoft.EntityFrameworkCore;
using Product_API.Domains.Categories;
using Product_API.Domains.Comments;
using Product_API.Domains.Discounts;
using Product_API.Domains.Products;

namespace Product_API.Databases;

public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }

    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<Category> Categories { get; set; }

    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(Configuration.ProductConfigure.Create());

        modelBuilder.ApplyConfiguration(Configuration.ProductVariantConfigure.Create());

        modelBuilder.ApplyConfiguration(Configuration.CategoryConfigure.Create());

        modelBuilder.ApplyConfiguration(Configuration.DiscountConfigure.Create());
    }
}
