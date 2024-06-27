using Carter;
using MediatR;
using Product_API.Domain.Categories;
using Product_API.Interfaces;
using Shared.Domain.Results;

namespace Product_API.Features.Categories;

public static class GetAllCategories
{
    public record Command : IRequest<Result<List<Category>>>;

    public class Handler(ICategoryRepository repository)
        : IRequestHandler<Command, Result<List<Category>>>
    {
        public async Task<Result<List<Category>>> Handle(
            Command request,
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
                    var command = new Command();
                    if (await sender.Send(command) is { IsFailure: true } result)
                    {
                        return Results.BadRequest(result.ErrorTypes);
                    }

                    return Results.Ok();
                }
            );
        }
    }
}
