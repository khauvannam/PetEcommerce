namespace Client_App.DTOs.Products.Responses;

public record ProductsInList
{
    public int ProductId { get; set; }
    public decimal DiscountPercent { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Quantity { get; set; }
    public int SoldQuantity { get; set; }
    public decimal TotalRating { get; set; }
};
