using Shared.Domain.Bases;

namespace Product_API.Domain.Products;

public sealed class ProductVariant : Entity
{
    public string VariantId => Id;
    public string VariantName { get; private set; }
    public string ImageUrl { get; private set; }
    public OriginalPrice OriginalPrice { get; private set; }
    public Discount Discount { get; private set; }
    public decimal DiscountedPrice => CalculateDiscountedPrice();

    private ProductVariant(string variantName, string imageUrl)
    {
        VariantName = variantName;
        ImageUrl = imageUrl;
    }

    public static ProductVariant Create(string variantName, string imageUrl) =>
        new(variantName, imageUrl);

    public void Update(string variantName, string imageUrl, OriginalPrice originalPrice) =>
        (VariantName, ImageUrl, OriginalPrice) = (variantName, imageUrl, originalPrice);

    public void SetPrice(decimal value) => OriginalPrice = OriginalPrice.Create(value);

    public void ApplyDiscount(decimal discountPercent) =>
        Discount = Discount.Create(discountPercent);

    private decimal CalculateDiscountedPrice()
    {
        return Discount.Percent == 0
            ? OriginalPrice.Value
            : OriginalPrice.Value - OriginalPrice.Value * Discount.Percent / 100;
    }
}

public class OriginalPrice : ValueObject
{
    private OriginalPrice(decimal value, Currency currency) =>
        (Value, Currency) = (value, currency);

    public decimal Value { get; private set; }
    public Currency Currency { get; private set; }

    public static OriginalPrice Create(decimal value, Currency currency = Currency.Usd)
    {
        if (value < 0)
            throw new ArgumentException("Price value must be positive");

        return new(value, currency);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Currency;
    }
}

public class Discount : ValueObject
{
    public decimal Percent { get; }

    private Discount(decimal percent)
    {
        if (percent is < 0 or > 100)
        {
            throw new ArgumentException("Discount percent must be between 0 and 100");
        }

        Percent = percent;
    }

    public static Discount Create(decimal percent) => new(percent);

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Percent;
    }
}

public enum Currency
{
    Usd = 0,
    Vnd = 1,
}
