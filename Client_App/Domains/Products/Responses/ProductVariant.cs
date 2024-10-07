namespace Client_App.Domains.Products.Responses;

public class ProductVariant
{
    public Guid VariantId { get; set; }
    public required string VariantName { get; set; }
    public int Quantity { get; set; }
    public required OriginalPrice OriginalPrice { get; set; }
    public Guid ProductId { get; set; }
}

public class OriginalPrice
{
    public required decimal Value { get; set; }
    public Currency Currency { get; set; }
}

public enum Currency
{
    Usd,
    Vnd,
}
