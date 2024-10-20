using Base.Results;
using MediatR;
using Product_API.Domain.Categories;
using Product_API.Interfaces;

namespace Product_API.Features.Categories;

public static class GetCategoryByEndpoint
{
    public sealed record Query(string Endpoint) : IRequest<Result<Category>>;

    public sealed class Handler(ICategoryRepository repository)
        : IRequestHandler<Query, Result<Category>>
    {
        public async Task<Result<Category>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            return await repository.GetByEndpointAsync(request);
        }
    }
}
