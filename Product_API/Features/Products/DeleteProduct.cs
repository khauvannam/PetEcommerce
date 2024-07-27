using BaseDomain.Results;
using Carter;
using MediatR;
using Product_API.Interfaces;
using Shared.Services;

namespace Product_API.Features.Products;

public static class DeleteProduct
{
    public record Command(string ProductId) : IRequest<Result>;

    public class Handler(IProductRepository repository, BlobService blobService)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await repository.GetByIdAsync(request.ProductId, cancellationToken);
            if (result.IsFailure)
            {
                return result;
            }

            var product = result.Value;
            var fileName = new Uri(product.ImageUrl).Segments[^1];
            await blobService.DeleteAsync(fileName);
            return await repository.DeleteAsync(product, cancellationToken);
        }
    }

    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete(
                "api/product/{productId}",
                async (ISender sender, string productId) =>
                {
                    var command = new Command(productId);
                    var result = await sender.Send(command);
                    if (result.IsFailure)
                    {
                        return Results.BadRequest(result.ErrorTypes);
                    }

                    return Results.Ok();
                }
            );
        }
    }
}
