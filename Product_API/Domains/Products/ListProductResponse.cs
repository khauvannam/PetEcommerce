namespace Product_API.Domains.Products;

public record ListProductResponse
{
    public Guid ProductId { get; init; }
    public DiscountPercent DiscountPercent { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string ImageUrl { get; init; } = null!;
    public decimal Price { get; init; }
    public DateTime CreatedAt { get; init; }
    public int Quantity { get; init; }
    public decimal TotalRating { get; init; }
};
