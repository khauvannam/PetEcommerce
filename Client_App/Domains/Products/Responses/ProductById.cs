using Client_App.Domains.Comments;

namespace Client_App.Domains.Products.Responses;

public sealed record ProductById(
    Guid ProductId,
    string Name,
    string Description,
    string ProductUseGuide,
    decimal TotalRating,
    int SoldQuantity,
    int TotalQuantity,
    decimal DiscountPercent,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    int CategoryId,
    List<ProductVariant> ProductVariants,
    List<string> ImageUrlList
);
