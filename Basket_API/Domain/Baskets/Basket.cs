using System.ComponentModel.DataAnnotations;
using Basket_API.Domain.BasketItems;
using Newtonsoft.Json;
using Shared.Domain.Bases;

namespace Basket_API.Domain.Baskets;

public class Basket : AggregateRoot
{
    private Basket() { }

    [MaxLength(255)]
    public string BasketId
    {
        get => Id;
        set => throw new ArgumentException("Can not set primary key");
    }

    [MaxLength(255)]
    public string CustomerId { get; private set; } = null!;

    public List<BasketItem> BasketItemsList { get; private init; } = null!;

    public static Basket Create(string customerId) =>
        new() { CustomerId = customerId, BasketItemsList = [] };

    public void UpdateBasket(BasketItemRequest basketItemRequest)
    {
        if (
            BasketItemsList.FirstOrDefault(bi =>
                bi.BasketItemId == basketItemRequest.BasketItemId
            ) is
            { } basketItem
        )
        {
            basketItem.SetPrice(basketItemRequest.Price);
            basketItem.SetOnSale(basketItemRequest.OnSale);
            basketItem.ChangeQuantity(basketItemRequest.Quantity);
            return;
        }
        var newBasketItem = BasketItem.Create(
            basketItemRequest.ProductId,
            basketItemRequest.VariantId,
            basketItemRequest.Name,
            Quantity.Create(basketItemRequest.Quantity),
            basketItemRequest.Price,
            basketItemRequest.ImageUrl,
            basketItemRequest.OnSale
        );
        BasketItemsList.Add(newBasketItem);
    }

    public void RemoveAllBasketItemNotExist(List<BasketItemRequest> basketItemRequests) =>
        BasketItemsList.RemoveAll(b => basketItemRequests.All(bi => bi.BasketItemId != b.BasketId));

    public void RemoveAllBasketItem() => BasketItemsList.Clear();
}
