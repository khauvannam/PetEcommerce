namespace Client_App.DTOs.Products.Requests;

public record ProductsSearchFilterRequest(
    string? SearchText = default,
    int Limit = 5,
    int Offset = default,
    decimal MinPrice = decimal.Zero,
    decimal MaxPrice = 10000M,
    string? SortBy = default,
    int Available = 1,
    int Direction = default
);
