using Basket_API.Domains.BasketItems;

namespace Basket_API.Domains.Baskets;

public record AddToBasketRequest(Guid CustomerId, List<BasketItemRequest> BasketItemRequests);
