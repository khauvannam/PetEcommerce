namespace Client_App.DTOs.Products.Responses;

public sealed record ProductById(
    int ProductId,
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
