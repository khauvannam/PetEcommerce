using Base;
using Base.Results;
using MediatR;
using Product_API.Domain.Products;
using Product_API.DTOs.Products;
using Product_API.Interfaces;

namespace Product_API.Features.Products;

public static class GetProductsBySearch
{
    public sealed record Query(ProductsSearchFilterRequest Filter)
        : IRequest<Result<Pagination<ListProductResponse>>>;

    public class Handler(IProductRepository repository)
        : IRequestHandler<Query, Result<Pagination<ListProductResponse>>>
    {
        public async Task<Result<Pagination<ListProductResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            return await repository.GetProductsBySearch(request.Filter);
        }
    }
}
