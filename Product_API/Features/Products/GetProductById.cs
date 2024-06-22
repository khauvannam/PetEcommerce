using Carter;
using MediatR;
using Product_API.Domain.Products;
using Product_API.Interfaces;
using Shared.Domain.Results;

namespace Product_API.Features.Products;

public static class GetProductById
{
    public record Command(string ProductId) : IRequest<Result<Product>>;

    public class Handler(IProductRepository repository) : IRequestHandler<Command, Result<Product>>
    {
        public async Task<Result<Product>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            return await repository.GetProductById(request.ProductId, cancellationToken);
        }
    }

    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/product/{productId}",
                async (string productId, ISender sender) =>
                {
                    var command = new Command(productId);
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
