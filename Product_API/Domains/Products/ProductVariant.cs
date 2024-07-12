using Shared.Domain.Bases;

namespace Product_API.Domains.Products;

public sealed class ProductVariant : Entity
{
    public string VariantId => Id;
    public string VariantName { get; private set; }
    public int InStock { get; private set; }
    public string ImageUrl { get; private set; }
    private OriginalPrice OriginalPrice { get; set; } = null!;
    private Discount Discount { get; set; } = null!;
    public decimal DiscountedPrice => CalculateDiscountedPrice();

    private ProductVariant(string variantName, string imageUrl, int inStock)
    {
        VariantName = variantName;
        ImageUrl = imageUrl;
        InStock = inStock;
    }

    public static ProductVariant Create(string variantName, string imageUrl, int inStock) =>
        new(variantName, imageUrl, inStock);

    public void Update(
        string variantName,
        string imageUrl,
        OriginalPrice originalPrice,
        Discount discount,
        int inStock
    )
    {
        VariantName = variantName;
        ImageUrl = imageUrl;
        OriginalPrice = originalPrice;
        Discount = discount;
        InStock = inStock;
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
    private OriginalPrice(decimal value, Currency currency) =>
        (Value, Currency) = (value, currency);

    public decimal Value { get; }
    public Currency Currency { get; }

    public static OriginalPrice Create(decimal value, Currency currency = Currency.Usd)
    {
        if (value < 0)
            throw new ArgumentException("Price value must be positive");

        return new(value, currency);
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

    private Discount(decimal percent)
    {
        if (percent is < 0 or > 100)
        {
            throw new ArgumentException("Discount percent must be between 0 and 100");
        }

        Percent = percent;
    }

    public static Discount Create(decimal percent) => new(percent);

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
