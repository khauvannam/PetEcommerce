using Basket_API.DTOs.BasketItems;

namespace Basket_API.DTOs.Baskets;

public record AddToBasketRequest(
    int CustomerId,
    UpdateBasketItemRequest UpdateBasketItemRequest
) { }
