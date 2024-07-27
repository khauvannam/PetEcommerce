using BaseDomain.Results;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product_API.Domains.Categories;
using Product_API.Interfaces;
using Shared.Services;

namespace Product_API.Features.Categories;

public static class CreateCategory
{
    public record Command(string CategoryName, string Description, IFormFile File)
        : IRequest<Result>;

    public class Handler(ICategoryRepository repository, BlobService blobService)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var fileName = await blobService.UploadFileAsync(request.File, "Category-");
            var category = Category.Create(request.CategoryName, request.Description, fileName);

            return await repository.CreateAsync(category);
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
