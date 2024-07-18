using BaseDomain.Results;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product_API.Domains.Categories;
using Product_API.Interfaces;

namespace Product_API.Features.Categories;

public static class UpdateCategory
{
    public record Command(string CategoryId, UpdateCategoryRequest UpdateCategoryRequest)
        : IRequest<Result<Category>>;

    public class Handler(ICategoryRepository repository)
        : IRequestHandler<Command, Result<Category>>
    {
        public async Task<Result<Category>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            return await repository.UpdateCategory(request);
        }
    }

    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut(
                "/api/category/{categoryId}",
                async (
                    string categoryId,
                    [FromForm] UpdateCategoryRequest updateCategoryRequest,
                    ISender sender
                ) =>
                {
                    var command = new Command(categoryId, updateCategoryRequest);
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
