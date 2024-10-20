using Product_API.Domain.Products;

namespace Product_API.DTOs.Products;

public record ProductByIdResponse(
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
