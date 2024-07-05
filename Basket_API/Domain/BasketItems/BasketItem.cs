using System.ComponentModel.DataAnnotations;
using Basket_API.Domain.Baskets;
using Shared.Domain.Bases;

namespace Basket_API.Domain.BasketItems;

public class BasketItem : Entity
{
    private BasketItem(
        string productId,
        string variantId,
        string name,
        decimal price,
        bool onSale,
        string imageUrl,
        Quantity quantity
    )
    {
        ProductId = productId;
        VariantId = variantId;
        Name = name;
        Price = price;
        OnSale = onSale;
        ImageUrl = imageUrl;
        Quantity = quantity;
        AddedAt = DateTime.Now;
    }

    public string BasketItemId => Id;

    [MaxLength(255)]
    public string ImageUrl { get; private set; }

    [MaxLength(255)]
    public string ProductId { get; private set; }

    [MaxLength(255)]
    public string VariantId { get; private set; }

    [MaxLength(255)]
    public string BasketId { get; private set; } = null!;
    public Basket Basket { get; init; } = null!;

    [MaxLength(255)]
    public string Name { get; private set; }
    private Quantity Quantity { get; set; }
    public decimal Price { get; private set; }
    public decimal TotalPrice => Price * Quantity.Value;
    public bool OnSale { get; private set; }
    public DateTime AddedAt { get; private set; }

    public static BasketItem Create(
        string productId,
        string variantId,
        string name,
        Quantity quantity,
        decimal price,
        string imageUrl,
        bool onSale = false
    )
    {
        return new(productId, variantId, name, price, onSale, imageUrl, quantity);
    }

    public void SetPrice(decimal price) => Price = price;

    public void SetOnSale(bool onSale = true)
    {
        OnSale = onSale;
    }

    public void ChangeQuantity(int quantity) => Quantity.Update(quantity);
}

public class Quantity : ValueObject
{
    private Quantity(int value)
    {
        if (value <= 0)
        {
            throw new ArgumentException("Quantity can't be negative");
        }
        Value = value;
    }

    public int Value { get; private set; }

    public static Quantity Create(int value) => new(value);

    public void Update(int value) =>
        Value = value > 0 ? value : throw new ArgumentException("Quantity can't be negative");

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
