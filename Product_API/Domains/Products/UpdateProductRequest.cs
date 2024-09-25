namespace Product_API.Domains.Products;

public record UpdateProductRequest(
    string Name,
    string Description,
    string ProductUseGuide,
    IFormFile? File,
    int CategoryId,
    List<ProductVariantRequest> ProductVariants,
    decimal Percent
);
