using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product_API.Domains.Products;
using Product_API.Interfaces;
using Shared.Domain.Results;

namespace Product_API.Features.Products;

public static class UpdateProduct
{
    public record Command(string ProductId, UpdateProductRequest UpdateProductRequest)
        : IRequest<Result<Product>>;

    public class Handler(IProductRepository repository) : IRequestHandler<Command, Result<Product>>
    {
        public async Task<Result<Product>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            return await repository.UpdateProduct(request, cancellationToken);
        }
    }

    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(
                "api/product/{productId}",
                async (
                    ISender sender,
                    string productId,
                    [FromBody] UpdateProductRequest updateProductRequest
                ) =>
                {
                    var command = new Command(productId, updateProductRequest);
                    var result = await sender.Send(command);
                    if (!result.IsFailure)
                    {
                        return Results.Ok(result.Value);
                    }

                    return Results.BadRequest(result.ErrorTypes);
                }
            );
        }
    }
}
