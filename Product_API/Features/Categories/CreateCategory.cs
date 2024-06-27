using Carter;
using MediatR;
using Product_API.Interfaces;
using Shared.Domain.Results;

namespace Product_API.Features.Categories;

public static class CreateCategory
{
    public record Command(string CategoryName, List<string> Details) : IRequest<Result>;

    public class Handler(ICategoryRepository repository) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            return await repository.CreateCategory(request);
        }
    }

    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost(
                "/api/category",
                async (ISender sender, Command command) =>
                {
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
