using BaseDomain.Results;
using Carter;
using MediatR;
using Product_API.Domains.Categories;
using Product_API.Interfaces;

namespace Product_API.Features.Categories;

public static class GetCategoryById
{
    public record Query(string CategoryId) : IRequest<Result<Category>>;

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

    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/api/category/{categoryId}",
                async (ISender sender, string categoryId) =>
                {
                    var query = new Query(categoryId);
                    var result = await sender.Send(query);
                    if (result.IsFailure)
                    {
                        return Results.BadRequest(result.ErrorTypes);
                    }

                    return Results.Ok(result.Value);
                }
            );
        }
    }
}
