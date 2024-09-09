using BaseDomain.Results;
using Carter;
using MediatR;
using Product_API.Domains.Categories;
using Product_API.Interfaces;

namespace Product_API.Features.Categories;

public static class GetAllCategories
{
    public record Query : IRequest<Result<List<Category>>>;

    public class Handler(ICategoryRepository repository)
        : IRequestHandler<Query, Result<List<Category>>>
    {
        public async Task<Result<List<Category>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            return await repository.GetAllAsync();
        }
    }
}
