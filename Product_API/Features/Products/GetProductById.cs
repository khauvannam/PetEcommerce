using BaseDomain.Results;
using MediatR;
using Product_API.Domains.Products;
using Product_API.Interfaces;

namespace Product_API.Features.Products;

public static class GetProductById
{
    public record Query(Guid ProductId) : IRequest<Result<Product>>;

    public class Handler(IProductRepository repository) : IRequestHandler<Query, Result<Product>>
    {
        public async Task<Result<Product>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            return await repository.GetInDetailByIdAsync(request.ProductId, cancellationToken);
        }
    }
}
