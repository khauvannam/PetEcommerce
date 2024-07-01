namespace Product_API.Domain.Products;

public record UpdateProductRequest(
    string Name,
    ProductCategory ProductCategory,
    List<ProductVariant> ProductVariants
);
