namespace Product_API.Domain.Products;

public sealed class ProductVariant
{
    public string VariantId = Guid.NewGuid().ToString();
    public string VariantName { get; private set; }
    public OriginalPrice OriginalPrice { get; private set; }
    public Discount Discount { get; private set; }
    public decimal DiscountedPrice => CalculateDiscountedPrice();

    private ProductVariant(string variantName, OriginalPrice originalPrice, Discount discount)
    {
        VariantName = variantName;
        OriginalPrice = originalPrice;
        Discount = discount;
    }

    public static ProductVariant Create(
        string variantName,
        OriginalPrice originalPrice,
        Discount discount
    ) => new(variantName, originalPrice, discount);

    public void Update(string variantName, OriginalPrice originalPrice) =>
        (VariantName, OriginalPrice) = (variantName, originalPrice);

    public void ApplyDiscount(Discount discount) => Discount = discount;

    private decimal CalculateDiscountedPrice()
    {
        return Discount.Percent == 0
            ? OriginalPrice.Value
            : OriginalPrice.Value - (OriginalPrice.Value * Discount.Percent / 100);
    }
}

public record OriginalPrice(decimal Value, Currency Currency = Currency.Usd);

public record Discount
{
    public decimal Percent { get; }

    public Discount(decimal percent)
    {
        if (percent is < 0 or > 100)
        {
            throw new ArgumentException("Discount percent must be between 0 and 100");
        }
        Percent = percent;
    }
}

public enum Currency
{
    Usd = 0,
    Vnd = 1,
}
