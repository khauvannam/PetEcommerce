using Base;
using Base.Results;
using Product_API.Domain.Products;
using Product_API.DTOs.Products;
using Product_API.Features.Products;

namespace Product_API.Interfaces;

public interface IProductRepository
{
    ValueTask<Result<Pagination<ListProductResponse>>> ListAllAsync(GetAllProducts.Query command);
    Task<Result<Product>> CreateAsync(Product product);
    Task<Result> DeleteAsync(Product product, CancellationToken cancellationToken);
    Task<Result<Product>> UpdateAsync(Product product, CancellationToken cancellationToken);
    Task<Result<Product>> GetByIdAsync(int productId, CancellationToken cancellationToken);

    Task<Result<ProductByIdResponse>> GetInDetailByIdAsync(
        int productId,
        CancellationToken cancellationToken
    );

    ValueTask<Result<Pagination<ListProductResponse>>> GetProductsBySearch(
        ProductsSearchFilterRequest filterRequest
    );
}
