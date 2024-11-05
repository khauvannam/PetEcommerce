namespace Basket_API.DTOs.BasketItems;

public record UpdateBasketItemRequest(
    int ProductId,
    int VariantId,
    string Name,
    int Quantity,
    decimal Price,
    string ImageUrl
);
