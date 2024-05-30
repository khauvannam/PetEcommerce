using Product_API.Domain;
using Product_API.Features.Products;
using Shared.Domain.Results;

namespace Product_API.Interfaces;

public interface IProductRepository
{
    Task<Result<Product>> CreateProduct(CreateProduct.Command command);
    Task<Result> DeleteProduct();
}
