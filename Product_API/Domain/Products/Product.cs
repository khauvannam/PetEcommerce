using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Domain.Bases;

namespace Product_API.Domain.Products;

public class Product : AggregateRoot
{
    private Product(string name, ProductCategory productCategory)
    {
        Name = name;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
        ProductCategory = productCategory;
    }

    [BsonId]
    public string ProductId => Id;
    public string Name { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public ProductCategory ProductCategory { get; private set; }
    public List<ProductVariant> ProductVariants { get; } = [];

    public static Product Create(string name, ProductCategory productCategory)
    {
        return new(name, productCategory);
    }

    public void UpdateProduct(string name, ProductCategory productCategory)
    {
        Name = name;
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
