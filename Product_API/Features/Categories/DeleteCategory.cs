﻿using BaseDomain.Results;
using Carter;
using MediatR;
using Product_API.Interfaces;
using Shared.Services;

namespace Product_API.Features.Categories;

public static class DeleteCategory
{
    public record Command(string CategoryId) : IRequest<Result>;

    public class Handler(ICategoryRepository repository, BlobService blobService)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await repository.GetByIdAsync(request.CategoryId);
            if (result.IsFailure)
            {
                return result;
            }

            var category = result.Value;
            var fileName = new Uri(category.ImageUrl).Segments[^1];
            await blobService.DeleteAsync(fileName);
            return await repository.DeleteAsync(category);
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
