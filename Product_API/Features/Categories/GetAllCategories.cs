using Carter;
using MediatR;
using Product_API.Domains.Categories;
using Product_API.Interfaces;
using Shared.Domain.Results;

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
            return await repository.GetAllCategories();
        }
    }

    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(
                "api/categories",
                async (ISender sender) =>
                {
                    var query = new Query();
                    if (await sender.Send(query) is { IsFailure: true } result)
                    {
                        return Results.BadRequest(result.ErrorTypes);
                    }

                    return Results.Ok();
                }
            );
        }
    }
}
