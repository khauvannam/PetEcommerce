using Client_App.Domains.Comments;

namespace Client_App.Domains.Products.Responses;

public class DetailedProduct
{
    public Guid ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ProductUseGuide { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public int SoldQuantity { get; set; }

    public DiscountPercent DiscountPercent { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public decimal TotalRating { get; set; }

    public int TotalQuantity { get; set; }

    public Guid CategoryId { get; set; }

    public List<ProductVariant> ProductVariants { get; } = [];

    public List<Comment> Comments { get; } = [];

    public HashSet<ProductBuyerId> ProductBuyerIds { get; } = [];
}

public class DiscountPercent { }
