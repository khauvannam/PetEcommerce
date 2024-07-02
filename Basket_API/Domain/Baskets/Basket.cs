using Basket_API.Domain.BasketItems;
using Shared.Domain.Bases;

namespace Basket_API.Domain.Baskets;

public class Basket : AggregateRoot
{
    private Basket(string customerId)
    {
        CustomerId = customerId;
    }

    public string BasketId => Id;
    public string CustomerId { get; private set; }
    public List<BasketItem> BasketItemsList { get; } = [];

    public static Basket Create(string customerId) => new(customerId);

    public void AddBasketItem(BasketItem basketItem)
    {
        BasketItemsList.Add(basketItem);
    }
}
