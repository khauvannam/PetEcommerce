using Base;
using Base.Results;
using MediatR;
using Product_API.DTOs.Products;
using Product_API.Interfaces;

namespace Product_API.Features.Products;

public static class GetAllProducts
{
    public sealed record Query(int? CategoryId, int Limit, int Offset, bool IsBestSeller)
        : IRequest<Result<Pagination<ListProductResponse>>>;

    public sealed class Handler(IProductRepository repository)
        : IRequestHandler<Query, Result<Pagination<ListProductResponse>>>
    {
        public async Task<Result<Pagination<ListProductResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            return await repository.ListAllAsync(request);
        }
    }
}
