namespace Client_App.Domains.Products.Responses;

public record ProductsInList
{
    public Guid ProductId { get; init; }
    public DiscountPercent DiscountPercent { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string ImageUrl { get; init; } = null!;
    public decimal Price { get; init; }
    public DateTime CreatedAt { get; init; }
    public int Quantity { get; init; }
    public int SoldQuantity { get; init; }
    public decimal TotalRating { get; init; }
};
