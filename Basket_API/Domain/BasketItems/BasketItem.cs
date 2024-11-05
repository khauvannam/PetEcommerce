using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Base.Bases;
using Basket_API.Domain.Baskets;

namespace Basket_API.Domain.BasketItems;

public class BasketItem : Entity
{
    private BasketItem() { }

    [MaxLength(255)]
    [JsonInclude]
    public int BasketItemId
    {
        get => Id;
        private init => Id = value;
    }

    [MaxLength(255)]
    public string ImageUrl { get; private set; }

    [MaxLength(255)]
    public int ProductId { get; private set; }

    [MaxLength(255)]
    public int VariantId { get; private set; }

    [MaxLength(255)]
    public int BasketId { get; init; }

    [Newtonsoft.Json.JsonIgnore]
    public Basket Basket { get; init; }

    [MaxLength(255)]
    public string Name { get; private set; }

    public Quantity Quantity { get; private set; }
    public decimal Price { get; private set; }
    public DateTime AddedAt { get; private set; }

    public static BasketItem Create(
        int productId,
        int variantId,
        string name,
        Quantity quantity,
        decimal price,
        string imageUrl
    )
    {
        return new BasketItem
        {
            ProductId = productId,
            VariantId = variantId,
            Name = name,
            Price = price,
            ImageUrl = imageUrl,
            Quantity = quantity,
            AddedAt = DateTime.Now,
        };
    }

    public decimal GetTotalPrice()
    {
        return Price * Quantity.Value;
    }

    public void SetPrice(decimal price)
    {
        Price = price;
    }

    public void ChangeQuantity(int quantity)
    {
        Quantity.Update(quantity);
    }
}

public class Quantity : ValueObject
{
    private Quantity() { }

    public int Value { get; private set; }

    public static Quantity Create(int value)
    {
        if (value <= 0)
            throw new ArgumentException("Quantity can't be negative");

        return new Quantity { Value = value };
    }

    public void Update(int value)
    {
        Value += value > 0 ? value : throw new ArgumentException("Quantity can't be negative");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
