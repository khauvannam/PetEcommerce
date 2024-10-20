using Base.Results;
using MediatR;
using Product_API.Domain.Categories;
using Product_API.Interfaces;

namespace Product_API.Features.Categories;

public static class GetCategoryById
{
    public record Query(int CategoryId) : IRequest<Result<Category>>;

    public class Handler(ICategoryRepository repository) : IRequestHandler<Query, Result<Category>>
    {
        public async Task<Result<Category>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            return await repository.GetByIdAsync(request.CategoryId);
        }
    }
}
