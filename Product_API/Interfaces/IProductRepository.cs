using Product_API.Features.Products;
using Shared.Entities.Results;

namespace Product_API.Interfaces;

public interface IProductRepository
{
    Task<Result<Entities.Product>> CreateProduct(CreateProduct.Command command);
    Task<Result> DeleteProduct();
}
