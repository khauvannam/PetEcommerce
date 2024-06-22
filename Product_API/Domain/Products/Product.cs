using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Product_API.Domain.Products;

public class Product
{
    private Product(string name, OriginalPrice originalPrice, ProductCategory productCategory)
    {
        ProductId = Guid.NewGuid().ToString();
        Name = name;
        OriginalPrice = originalPrice;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
        ProductCategory = productCategory;
    }

    [BsonId]
    public string ProductId { get; }
    public string Name { get; private set; }
    public OriginalPrice OriginalPrice { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public ProductCategory ProductCategory { get; private set; }
    public List<ProductVariant> ProductVariants { get; } = [];
    public static Product Create(string name, OriginalPrice originalPrice, ProductCategory productCategory)
    {
        return new(name, originalPrice, productCategory);
    }

    public void UpdateProduct(string name, OriginalPrice originalPrice, ProductCategory productCategory)
    {
        Name = name;
        OriginalPrice = originalPrice;
        UpdatedAt = DateTime.Now;
        UpdateCategory(productCategory);
    }

    public void AddProductVariants(ProductVariant productVariant)
    {
        ProductVariants.Add(productVariant);
    }

    private void UpdateCategory(ProductCategory other)
    {
        if (ProductCategory.Equals(other))
        {
            return;
        }

        ProductCategory = other;
    }
    
}
public record ProductCategory(string CategoryId, BsonDocument Details);
