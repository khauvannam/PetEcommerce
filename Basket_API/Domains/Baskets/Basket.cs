using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BaseDomain.Bases;
using Basket_API.Domains.BasketItems;

namespace Basket_API.Domains.Baskets;

public class Basket : AggregateRoot
{
    private Basket() { }

    [MaxLength(255)]
    [JsonInclude]
    public string BasketId
    {
        get => Id;
        private set => Id = value;
    }

    [MaxLength(255)]
    [JsonInclude]
    public string CustomerId { get; private set; } = null!;

    [JsonInclude]
    public HashSet<BasketItem> BasketItemsList { get; private init; } = null!;

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
            basketItem.ChangeQuantity(basketItemRequest.Quantity);
            return;
        }
        var newBasketItem = BasketItem.Create(
            basketItemRequest.ProductId,
            basketItemRequest.VariantId,
            basketItemRequest.Name,
            Quantity.Create(basketItemRequest.Quantity),
            basketItemRequest.Price,
            basketItemRequest.ImageUrl
        );

        BasketItemsList.Add(newBasketItem);
    }

    public void RemoveAllBasketItemNotExist(List<BasketItemRequest> basketItemRequests) =>
        BasketItemsList.RemoveWhere(b =>
            basketItemRequests.All(bi => bi.BasketItemId != b.BasketId)
        );

    public void RemoveAllBasketItem() => BasketItemsList.Clear();
}
