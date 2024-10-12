using Base.Results;
using MediatR;
using Product_API.Domains.Products;
using Product_API.DTOs.Products;
using Product_API.Interfaces;

namespace Product_API.Features.Products;

public static class GetProductById
{
    public record Query(Guid ProductId) : IRequest<Result<ProductByIdResponse>>;

    public class Handler(IProductRepository repository)
        : IRequestHandler<Query, Result<ProductByIdResponse>>
    {
        public async Task<Result<ProductByIdResponse>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            return await repository.GetInDetailByIdAsync(request.ProductId, cancellationToken);
        }
    }
}
