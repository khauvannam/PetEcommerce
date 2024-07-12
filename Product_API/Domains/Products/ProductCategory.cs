using Shared.Domain.Bases;

namespace Product_API.Domains.Products;

public sealed class ProductCategory : ValueObject
{
    private ProductCategory() { }

    public string ProductCategoryId { get; private set; }

    public Dictionary<string, string> Details { get; private set; }

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

public record ProductCategoryDto(string ProductCategoryId, Dictionary<string, string> Details);
