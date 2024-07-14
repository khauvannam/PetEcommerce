using BaseDomain.Bases;

namespace Product_API.Domains.Products;

public sealed class ProductCategory : ValueObject
{
    private ProductCategory() { }

    public string ProductCategoryId { get; private set; } = null!;

    public Dictionary<string, string> Details { get; private set; } = null!;

    public static ProductCategory Create(
        string productCategoryId,
        Dictionary<string, string> details
    )
    {
        return new() { ProductCategoryId = productCategoryId, Details = details };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ProductCategoryId;
        yield return Details;
    }
}
