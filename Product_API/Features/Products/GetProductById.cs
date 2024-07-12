using Carter;
using MediatR;
using Product_API.Domains.Products;
using Product_API.Interfaces;
using Shared.Domain.Results;

namespace Product_API.Features.Products;

public static class GetProductById
{
    public record Query(string ProductId) : IRequest<Result<Product>>;

    public class Handler(IProductRepository repository) : IRequestHandler<Query, Result<Product>>
    {
        public async Task<Result<Product>> Handle(
            Query request,
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
                "api/product/{productId}",
                async (string productId, ISender sender) =>
                {
                    var query = new Query(productId);
                    var result = await sender.Send(query);
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
