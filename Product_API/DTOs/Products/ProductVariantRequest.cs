namespace Product_API.DTOs.Products;

public sealed record ProductVariantRequest(string VariantName, decimal OriginalPrice, int Quantity);
