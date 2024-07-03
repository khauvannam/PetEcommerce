using Product_API.Domain.Products;
using Product_API.Features.Products;
using Shared.Domain.Results;

namespace Product_API.Interfaces;

public interface IProductRepository
{
    Task<Result<List<Product>>> ListAllProducts(ListAllProducts.Query command);
    Task<Result<Product>> CreateProduct(CreateProduct.Command command);
    Task<Result> DeleteProduct(string productId, CancellationToken cancellationToken);
    Task<Result<Product>> UpdateProduct(
        UpdateProduct.Command command,
        CancellationToken cancellationToken
    );
    Task<Result<Product>> GetProductById(string productId, CancellationToken cancellationToken);
}
