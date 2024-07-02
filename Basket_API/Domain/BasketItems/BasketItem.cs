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
        ;
        Price = price;
        OnSale = onSale;
        ImageUrl = imageUrl;
        Quantity = quantity;
        AddedAt = DateTime.Now;
    }

    public string BasketItemId => Id;
    public string ImageUrl { get; private set; }
    public string ProductId { get; private set; }
    public string VariantId { get; private set; }
    public string BasketId { get; private set; } = null!;
    public Basket Basket { get; init; } = null!;
    public string Name { get; private set; }
    private Quantity Quantity { get; set; }
    public decimal Price { get; private set; }
    public bool OnSale { get; private set; }
    public DateTime AddedAt { get; private set; }

    public static BasketItem Create(
        string productId,
        string variantId,
        string name,
        Quantity quantity,
        decimal price,
        bool onSale,
        string imageUrl
    )
    {
        return new(productId, variantId, name, price, onSale, imageUrl, quantity);
    }

    public void Update(
        string productId,
        string variantId,
        int quantity,
        decimal price,
        bool onSale,
        string imageUrl
    )
    {
        ProductId = productId;
        VariantId = variantId;
        Quantity.Update(quantity);
        Price = price;
        OnSale = onSale;
        ImageUrl = imageUrl;
    }

    public void SetPrice(decimal price) => Price = price;

    public void AddToBasket(string basketId) => BasketId = basketId;

    public void SetOnSale(bool onSale)
    {
        OnSale = onSale;
    }
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

    private int Value { get; set; }

    public static Quantity Create(int value) => new(value);

    public void Update(int value) =>
        Value = value > 0 ? value : throw new ArgumentException("Quantity can't be negative");

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
