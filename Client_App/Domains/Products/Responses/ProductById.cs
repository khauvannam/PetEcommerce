using Client_App.Domains.Comments;

namespace Client_App.Domains.Products.Responses;

public sealed class ProductById
{
    public Guid ProductId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? ProductUseGuide { get; set; }

    public List<string> ImageUrlList { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;

    public int SoldQuantity { get; set; }

    public DiscountPercent DiscountPercent { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public decimal TotalRating { get; set; }

    public int TotalQuantity { get; set; }

    public int CategoryId { get; set; }

    public decimal Price { get; set; }

    public List<ProductVariant> ProductVariants { get; set; } = null!;

    public List<Comment> Comments { get; set; } = null!;

    public HashSet<ProductBuyerId> ProductBuyerIds { get; set; } = null!;
}

public record DiscountPercent(decimal Value);
