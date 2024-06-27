using Carter;
using MediatR;
using Product_API.Interfaces;
using Shared.Domain.Results;

namespace Product_API.Features.Categories;

public static class DeleteCategory
{
    public record Command(string CategoryId) : IRequest<Result>;

    public class Handler(ICategoryRepository repository) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            return await repository.DeleteCategory(request);
        }
    }

    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete(
                "/api/category/{categoryId}",
                async (ISender sender, string categoryId) =>
                {
                    var command = new Command(categoryId);
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
