using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Domain.Bases;

namespace Product_API.Domain.Products;

public class Product : AggregateRoot
{
    private Product(string name, string description, ProductCategory productCategory)
    {
        Name = name;
        Description = description;
        ProductCategory = productCategory;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    [BsonId]
    public string ProductId => Id;
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public ProductCategory ProductCategory { get; private set; }
    private List<ProductVariant> ProductVariants { get; } = [];

    public static Product Create(string name, string description, ProductCategory productCategory)
    {
        return new(name, description, productCategory);
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

    private Func<ProductVariant, bool> FilterFor =>
        updatedVariants => ProductVariants.Contains(updatedVariants);

    private void UpdateCategory(ProductCategory other)
    {
        if (ProductCategory.Equals(other))
        {
            return;
        }

        ProductCategory = other;
    }
}

public class ProductCategory : ValueObject
{
    public string ProductCategoryId { get; }
    public BsonDocument Details { get; set; }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return ProductCategoryId;
        yield return Details;
    }
}
