namespace Basket_API.DTOs.BasketItems;

public record BasketItemRequest(
    int? BasketItemId,
    string ProductId,
    string VariantId,
    string Name,
    int Quantity,
    decimal Price,
    string ImageUrl
);
