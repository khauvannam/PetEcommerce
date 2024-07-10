using Shared.Domain.Bases;

namespace Product_API.Domain.Products;

public sealed class ProductCategory : ValueObject
{
    public string ProductCategoryId { get; set; }

    public string Details { get; set; }

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
