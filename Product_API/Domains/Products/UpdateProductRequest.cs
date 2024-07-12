namespace Product_API.Domains.Products;

public record UpdateProductRequest(
    string Name,
    string Description,
    string ProductUseGuide,
    ProductCategory ProductCategory,
    List<ProductVariant> ProductVariants
);
