using System.ComponentModel.DataAnnotations;
using Basket_API.Domain.BasketItems;
using Newtonsoft.Json;
using Shared.Domain.Bases;

namespace Basket_API.Domain.Baskets;

public class Basket : AggregateRoot
{
    [JsonConstructor]
    private Basket() { }

    [MaxLength(255)]
    public string BasketId
    {
        get => Id;
        set => throw new ArgumentException("Can not set primary key");
    }

    [MaxLength(255)]
    public string CustomerId { get; private set; } = null!;

    [JsonIgnore]
    public List<BasketItem> BasketItemsList { get; private init; } = null!;

    public static Basket Create(string customerId) =>
        new() { CustomerId = customerId, BasketItemsList = [] };

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
        oldBasketItem.SetPrice(newBasketItem.Price);
        oldBasketItem.SetOnSale(newBasketItem.OnSale);
        oldBasketItem.ChangeQuantity(newBasketItem.Quantity);
    }

    public void RemoveAllBasketItemNotExist(List<BasketItemRequest> basketItemRequests)
    {
        BasketItemsList.RemoveAll(b => basketItemRequests.All(bi => bi.BasketItemId != b.BasketId));
    }

    public void RemoveAllBasketItem() => BasketItemsList.Clear();
}
