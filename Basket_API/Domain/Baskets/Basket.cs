using System.ComponentModel.DataAnnotations;
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

    [MaxLength(255)]
    public string CustomerId { get; private set; }
    public List<BasketItem> BasketItemsList { get; private set; } = null!;

    public static Basket Create(string customerId) => new(customerId);

    public void CreateNewBasketItem(BasketItem basketItem)
    {
        BasketItemsList = [];
        AddBasketItem(basketItem);
    }

    public void UpdateBasket(BasketItemRequest newBasketItem)
    {
        var oldBasketItem = BasketItemsList.FirstOrDefault(bi =>
            bi.BasketItemId == newBasketItem.BasketItemId
        );

        if (oldBasketItem is null)
        {
            var basketItem = BasketItem.Create(
                newBasketItem.ProductId,
                newBasketItem.VariantId,
                newBasketItem.Name,
                Quantity.Create(newBasketItem.Quantity),
                newBasketItem.Price,
                newBasketItem.ImageUrl,
                newBasketItem.OnSale
            );
            BasketItemsList.Add(basketItem);
            return;
        }
        oldBasketItem.ChangeQuantity(newBasketItem.Quantity);
    }

    public void RemoveAllBasketItemNotExist(List<BasketItemRequest> basketItemRequests)
    {
        BasketItemsList.RemoveAll(b => basketItemRequests.All(bi => bi.BasketItemId != b.BasketId));
    }

    public void RemoveAllBasketItem() => BasketItemsList.Clear();

    private void AddBasketItem(BasketItem basketItem) => BasketItemsList.Add(basketItem);
}
