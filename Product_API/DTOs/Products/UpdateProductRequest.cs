namespace Product_API.DTOs.Products;

public record UpdateProductRequest(
    string Name,
    string Description,
    string ProductUseGuide,
    List<IFormFile> Images,
    int CategoryId,
    List<ProductVariantRequest> ProductVariants,
    decimal Percent
);
