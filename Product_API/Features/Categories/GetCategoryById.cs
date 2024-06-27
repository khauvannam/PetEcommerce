using Carter;
using MediatR;
using Product_API.Domain.Categories;
using Product_API.Interfaces;
using Shared.Domain.Results;

namespace Product_API.Features.Categories;

public static class GetCategoryById
{
    public record Command(string CategoryId) : IRequest<Result<Category>>;

    public class Handler(ICategoryRepository repository)
        : IRequestHandler<Command, Result<Category>>
    {
        public async Task<Result<Category>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            return await repository.GetCategoryById(request);
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
                    var command = new Command(categoryId);
                    var result = await sender.Send(command);
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
