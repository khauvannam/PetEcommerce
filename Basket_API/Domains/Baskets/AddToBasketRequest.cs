using Basket_API.Domains.BasketItems;
using Basket_API.DTOs.BasketItems;

namespace Basket_API.Domains.Baskets;

public record AddToBasketRequest(Guid CustomerId, List<BasketItemRequest> BasketItemRequests);
