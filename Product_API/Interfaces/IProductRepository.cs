using BaseDomain.Results;
using Product_API.Domains.Products;
using Product_API.Features.Products;

namespace Product_API.Interfaces;

public interface IProductRepository
{
    ValueTask<Result<List<Product>>> ListAllAsync(GetAllProducts.Query command);
    Task<Result<Product>> CreateAsync(Product product);
    Task<Result> DeleteAsync(Product product, CancellationToken cancellationToken);
    Task<Result<Product>> UpdateAsync(Product product, CancellationToken cancellationToken);
    Task<Result<Product>> GetByIdAsync(string productId, CancellationToken cancellationToken);
}
