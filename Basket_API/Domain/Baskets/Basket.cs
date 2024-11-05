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

    [JsonInclude]
    public HashSet<BasketItem> BasketItemsList { get; private init; } = null!;

    public static Basket Create(int customerId)
    {
        return new Basket { CustomerId = customerId, BasketItemsList = [] };
    }

    public void UpdateBasket(UpdateBasketItemRequest updateBasketItemRequest)
    {
        if (
            BasketItemsList.FirstOrDefault(bi =>
                bi.ProductId == updateBasketItemRequest.ProductId
                && bi.VariantId == updateBasketItemRequest.VariantId
            ) is
            { } basketItem
        )
        {
            basketItem.SetPrice(updateBasketItemRequest.Price);
            basketItem.ChangeQuantity(updateBasketItemRequest.Quantity);
            return;
        }

        var newBasketItem = BasketItem.Create(
            updateBasketItemRequest.ProductId,
            updateBasketItemRequest.VariantId,
            updateBasketItemRequest.Name,
            Quantity.Create(updateBasketItemRequest.Quantity),
            updateBasketItemRequest.Price,
            updateBasketItemRequest.ImageUrl
        );

        BasketItemsList.Add(newBasketItem);
    }

    public void RemoveAllBasketItemNotExist(List<UpdateBasketItemRequest> basketItemRequests)
    {
        BasketItemsList.RemoveWhere(b =>
            basketItemRequests.All(bi => bi.ProductId != b.ProductId && bi.VariantId != b.VariantId)
        );
    }

    public void RemoveAllBasketItem()
    {
        BasketItemsList.Clear();
    }
}
