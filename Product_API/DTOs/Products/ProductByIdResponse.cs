using Product_API.Domains.Products;

namespace Product_API.DTOs.Products;

public record ProductByIdResponse(
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
