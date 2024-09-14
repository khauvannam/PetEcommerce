namespace Product_API.Domains.Products;

public record UpdateProductRequest(
    string Name,
    string Description,
    string ProductUseGuide,
    IFormFile? File,
    Guid CategoryId,
    List<ProductVariantRequest> ProductVariants,
    decimal Percent
);
