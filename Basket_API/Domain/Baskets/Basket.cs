using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Base.Bases;
using Basket_API.Domain.BasketItems;
using Basket_API.DTOs.BasketItems;

namespace Basket_API.Domain.Baskets;

public class Basket : AggregateRoot
{
    private Basket() { }

    [MaxLength(255)]
    [JsonInclude]
    public int BasketId
    {
        get => Id;
        private init => Id = value;
    }

    [MaxLength(255)]
    [JsonInclude]
    public int CustomerId { get; private set; }

    public BasketStatus Status { get; private set; }

    [JsonInclude]
    public HashSet<BasketItem> BasketItemsList { get; private init; }

    public static Basket Create(int customerId)
    {
        return new Basket
        {
            CustomerId = customerId,
            BasketItemsList = [],
            Status = BasketStatus.Preparing,
        };
    }

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

    public void RemoveAllBasketItemNotExist(List<BasketItemRequest> basketItemRequests)
    {
        BasketItemsList.RemoveWhere(b =>
            basketItemRequests.All(bi => bi.BasketItemId != b.BasketId)
        );
    }

    public void RemoveAllBasketItem()
    {
        BasketItemsList.Clear();
    }
}

public enum BasketStatus
{
    Preparing,
    Fulfilled,
}
