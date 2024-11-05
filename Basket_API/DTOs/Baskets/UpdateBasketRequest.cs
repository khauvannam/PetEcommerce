using Basket_API.DTOs.BasketItems;

namespace Basket_API.DTOs.Baskets;

public record UpdateBasketRequest(int CustomerId, List<UpdateBasketItemRequest> BasketItemRequests);
