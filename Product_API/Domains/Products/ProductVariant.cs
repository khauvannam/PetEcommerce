using System.ComponentModel.DataAnnotations;
using BaseDomain.Bases;

namespace Product_API.Domains.Products;

public sealed class ProductVariant : Entity
{
    [Key]
    public string VariantId
    {
        get => Id;
        private set => Id = value;
    }

    public string VariantName { get; private set; }
    public int Quantity { get; private set; }
    private OriginalPrice OriginalPrice { get; set; } = null!;
    private Discount Discount { get; set; } = null!;
    public decimal DiscountedPrice => CalculateDiscountedPrice();

    private ProductVariant() { }

    public static ProductVariant Create(string variantName, int quantity) =>
        new() { VariantName = variantName, Quantity = quantity };

    public void Update(
        string variantName,
        OriginalPrice originalPrice,
        Discount discount,
        int quantity
    )
    {
        VariantName = variantName;
        OriginalPrice = originalPrice;
        Discount = discount;
        Quantity = quantity;
    }

    public void SetPrice(decimal value) => OriginalPrice = OriginalPrice.Create(value);

    public void ApplyDiscount(decimal discountPercent)
    {
        Discount = Discount.Create(discountPercent);
    }

    private decimal CalculateDiscountedPrice() =>
        Discount.Percent == 0
            ? OriginalPrice.Value
            : OriginalPrice.Value - OriginalPrice.Value * Discount.Percent / 100;
}

public class OriginalPrice : ValueObject
{
    private OriginalPrice() { }

    public decimal Value { get; private set; }
    public Currency Currency { get; private set; }

    public static OriginalPrice Create(decimal value, Currency currency = Currency.Usd)
    {
        if (value <= 0)
            throw new ArgumentException("Price value must be positive");

        return new() { Value = value, Currency = currency };
    }

    public void Update(decimal value, Currency currency = Currency.Usd)
    {
        if (value <= 0)
            throw new ArgumentException("Price value must be positive");
        Value = value;
        Currency = currency;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Currency;
    }
}

public class Discount : ValueObject
{
    public decimal Percent { get; private set; }

    private Discount() { }

    public static Discount Create(decimal percent)
    {
        if (percent is < 0 or > 100)
        {
            throw new ArgumentException("Discount percent must be between 0 and 100");
        }

        return new() { Percent = percent };
    }

    public void Update(decimal percent)
    {
        if (percent is < 0 or > 100)
        {
            throw new ArgumentException("Discount percent must be between 0 and 100");
        }

        Percent = percent;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Percent;
    }
}

public enum Currency
{
    Usd = 0,
    Vnd = 1,
}
