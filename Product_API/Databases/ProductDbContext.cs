using Microsoft.EntityFrameworkCore;
using Product_API.Domains.Categories;
using Product_API.Domains.Products;

namespace Product_API.Databases;

public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) { }
}
