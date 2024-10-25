namespace Product_API.DTOs.Products;

public record ProductsSearchFilterRequest(
    string? SearchText = default,
    int Limit = 5,
    decimal MinPrice = decimal.Zero,
    decimal MaxPrice = 10000M,
    string? SortBy = default,
    bool Available = true,
    int Offset = default,
    bool IsDesc = default
);
