using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Product_API.Entities;

public class Product
{
    private Product(string name, Price price, ProductCategory productCategory)
    {
        ProductId = Guid.NewGuid().ToString();
        Name = name;
        Price = price;
        ProductCategory = productCategory;
    }

    [BsonId]
    public string ProductId { get; }
    public string Name { get; private set; }
    public Price Price { get; private set; }
    public ProductCategory ProductCategory { get; private set; }

    public static Product Create(string name, Price price, ProductCategory productCategory)
    {
        return new(name, price, productCategory);
    }

    public void UpdateProduct(string name, Price price, ProductCategory productCategory)
    {
        Name = name;
        Price = price;
        UpdateCategory(productCategory);
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

public record Price(decimal Value, string Currency = "USD");

public record ProductCategory(string CategoryId, BsonDocument Details);
