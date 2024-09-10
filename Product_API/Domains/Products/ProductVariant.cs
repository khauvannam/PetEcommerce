using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BaseDomain.Bases;

namespace Product_API.Domains.Products;

public sealed class ProductVariant : Entity
{
    private ProductVariant() { }

    [JsonInclude]
    [MaxLength(255)]
    public string VariantId
    {
        get => Id;
        private set => Id = value;
    }

    [MaxLength(500)]
    public string VariantName { get; private set; } = null!;

    public int Quantity { get; private set; }
    public OriginalPrice OriginalPrice { get; set; } = null!;

    [JsonInclude]
    public decimal DiscountPercent { get; private set; }

    [JsonInclude]
    [MaxLength(255)]
    public string ProductId { get; set; } = null!;

    [Newtonsoft.Json.JsonIgnore]
    public Product Product { get; set; } = null!;

    public static ProductVariant Create(string variantName, int quantity)
    {
        return new ProductVariant { VariantName = variantName, Quantity = quantity };
    }

    public void Update(string variantName, OriginalPrice originalPrice, int quantity)
    {
        VariantName = variantName;
        OriginalPrice = originalPrice;
        Quantity = quantity;
    }

    public void SetPrice(decimal value)
    {
        OriginalPrice = OriginalPrice.Create(value);
    }

    public void SetDiscountPercent(decimal percent)
    {
        DiscountPercent = percent;
    }
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

        return new OriginalPrice { Value = value, Currency = currency };
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

public enum Currency
{
    Usd = 0,
    Vnd = 1,
}
