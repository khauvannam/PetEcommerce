namespace Product_API.Domain.Products;

public record ProductVariantRequest(
    string VariantName,
    string ImageUrl,
    decimal OriginalPrice,
    decimal DiscountPercent,
    int InStock
);
