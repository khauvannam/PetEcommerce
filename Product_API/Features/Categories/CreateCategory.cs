using BaseDomain.Results;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product_API.Interfaces;

namespace Product_API.Features.Categories;

public static class CreateCategory
{
    public record Command(
        string CategoryName,
        string Description,
        IFormFile File,
        List<string> Details
    ) : IRequest<Result>;

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
                    async (ISender sender, [FromForm] Command command) =>
                    {
                        if (await sender.Send(command) is { IsFailure: true } result)
                        {
                            return Results.BadRequest(result.ErrorTypes);
                        }

                        return Results.Ok();
                    }
                )
                .DisableAntiforgery();
        }
    }
}
