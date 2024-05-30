namespace Product_API.Domain.Variants;

public abstract class ProductVariant(string variantName, decimal price)
{
    public string VariantId = Guid.NewGuid().ToString();

    public string VariantName { get; set; } = variantName;
    public decimal Price { get; set; } = price;
}
