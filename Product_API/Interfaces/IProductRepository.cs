using BaseDomain.Results;
using Product_API.Domains.Products;
using Product_API.Features.Products;

namespace Product_API.Interfaces;

public interface IProductRepository
{
    ValueTask<Result<List<Product>>> ListAllProducts(GetAllProducts.Query command);
    Task<Result<Product>> CreateProduct(CreateProduct.Command command);
    Task<Result> DeleteProduct(string productId, CancellationToken cancellationToken);
    Task<Result<Product>> UpdateProduct(
        UpdateProduct.Command command,
        CancellationToken cancellationToken
    );
    Task<Result<Product>> GetProductById(string productId, CancellationToken cancellationToken);
}
