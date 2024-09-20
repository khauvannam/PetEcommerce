namespace Client_App.Domains.Products.Responses;

public class ProductBuyerId
{
    public Guid ProductId { get; init; }
    public Guid BuyerId { get; private init; }
}
