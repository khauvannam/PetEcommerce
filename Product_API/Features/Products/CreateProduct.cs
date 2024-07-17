using BaseDomain.Results;
using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product_API.Domains.Products;
using Product_API.Interfaces;

namespace Product_API.Features.Products;

public static class CreateProduct
{
    public record Command(
        string Name,
        string Description,
        string ProductUseGuide,
        IFormFile File,
        ProductCategory ProductCategory,
        List<ProductVariantRequest> ProductVariants
    ) : IRequest<Result<Product>>;

    public class Handler(IProductRepository repository) : IRequestHandler<Command, Result<Product>>
    {
        public async Task<Result<Product>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            return await repository.CreateProduct(request);
        }
    }

    public class Validator : AbstractValidator<Product>;

    public class EndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost(
                    "/api/product",
                    async (ISender sender, [FromForm] Command command) =>
                    {
                        var result = await sender.Send(command);
                        if (result.IsFailure)
                        {
                            return Results.BadRequest(result.ErrorTypes);
                        }

                        return Results.Ok(result.Value);
                    }
                )
                .DisableAntiforgery();
        }
    }
}
