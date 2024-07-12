namespace Basket_API.Domains.BasketItems;

public record BasketItemRequest(
    string? BasketItemId,
    string ProductId,
    string VariantId,
    string Name,
    int Quantity,
    decimal Price,
    bool OnSale,
    string ImageUrl
);
