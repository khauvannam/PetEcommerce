namespace Product_API.Domains.Products;

public sealed record ProductVariantRequest(
    string VariantName,
    decimal OriginalPrice,
    decimal DiscountPercent,
    int InStock
);
