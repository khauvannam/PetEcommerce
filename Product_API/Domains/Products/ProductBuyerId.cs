namespace Product_API.Domains.Products;

public class ProductBuyerId
{
    private ProductBuyerId() { }

    public Guid ProductId { get; init; }
    public Guid BuyerId { get; private init; }
    public Product Product { get; init; } = null!;

    public static ProductBuyerId Create(Guid buyerId)
    {
        return new ProductBuyerId { BuyerId = buyerId };
    }
}
