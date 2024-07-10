using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Domain.Bases;

namespace Product_API.Domain.Products;

public sealed class ProductCategory : ValueObject
{
    [BsonElement("CategoryId")]
    public string ProductCategoryId { get; set; }

    [BsonElement("Details")]
    public BsonDocument Details { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ProductCategoryId;
        yield return Details;
    }
}

public sealed class ProductCategoryDto
{
    public string ProductCategoryId { get; set; } = null!;

    public Dictionary<string, string> Details { get; set; } = null!;
}
