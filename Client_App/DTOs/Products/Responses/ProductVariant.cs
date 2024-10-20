namespace Client_App.DTOs.Products.Responses;

public class ProductVariant
{
    public int VariantId { get; set; }
    public required string VariantName { get; set; }
    public int Quantity { get; set; }
    public required OriginalPrice OriginalPrice { get; set; }
    public int ProductId { get; set; }
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
