namespace Basket_API.DTOs.BasketItems;

public record BasketItemRequest(
    Guid? BasketItemId,
    string ProductId,
    string VariantId,
    string Name,
    int Quantity,
    decimal Price,
    string ImageUrl
);
