namespace Client_App.Domains.Products.Responses;

public class ProductVariant
{
    public Guid VariantId { get; set; }
    public string VariantName { get; set; } = null!;
    public int Quantity { get; set; }
    public OriginalPrice OriginalPrice { get; set; } = null!;
    public Guid ProductId { get; set; }
}

public class OriginalPrice
{
    public decimal Value { get; set; }
    public Currency Currency { get; set; }
}

public enum Currency
{
    Usd,
    Vnd,
}
