using BasedDomain;
using BasedDomain.Results;
using Product_API.Domains.Products;
using Product_API.DTOs.Products;
using Product_API.Features.Products;

namespace Product_API.Interfaces;

public interface IProductRepository
{
    ValueTask<Result<Pagination<ListProductResponse>>> ListAllAsync(GetAllProducts.Query command);
    Task<Result<Product>> CreateAsync(Product product);
    Task<Result> DeleteAsync(Product product, CancellationToken cancellationToken);
    Task<Result<Product>> UpdateAsync(Product product, CancellationToken cancellationToken);
    Task<Result<Product>> GetByIdAsync(Guid productId, CancellationToken cancellationToken);
    Task<Result<Product>> GetInDetailByIdAsync(Guid productId, CancellationToken cancellationToken);
}
