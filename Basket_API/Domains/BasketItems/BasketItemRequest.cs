namespace Basket_API.Domains.BasketItems;

public record BasketItemRequest(
    Guid? BasketItemId,
    string ProductId,
    string VariantId,
    string Name,
    int Quantity,
    decimal Price,
    string ImageUrl
);
