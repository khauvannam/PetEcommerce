using Base;
using Base.Results;
using MediatR;
using Product_API.Domain.Categories;
using Product_API.Interfaces;

namespace Product_API.Features.Categories;

public static class GetAllCategories
{
    public record Query : IRequest<Result<Pagination<Category>>>;

    public class Handler(ICategoryRepository repository)
        : IRequestHandler<Query, Result<Pagination<Category>>>
    {
        public async Task<Result<Pagination<Category>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            return await repository.GetAllAsync();
        }
    }
}
