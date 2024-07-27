using BaseDomain.Results;
using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product_API.Domains.Products;
using Product_API.Interfaces;
using Shared.Errors;
using Shared.Services;

namespace Product_API.Features.Products;

public static class CreateProduct
{
    public record Command(
        string Name,
        string Description,
        string ProductUseGuide,
        IFormFile File,
        string CategoryId,
        List<ProductVariantRequest> ProductVariants
    ) : IRequest<Result<Product>>;

    public class Handler(IProductRepository repository, BlobService blobService)
        : IRequestHandler<Command, Result<Product>>
    {
        public async Task<Result<Product>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var fileName = await blobService.UploadFileAsync(request.File, "Product-");

            if (string.IsNullOrEmpty(fileName))
            {
                return Result.Failure<Product>(BlobErrors.ErrorUploadFile());
            }

            var product = Product.Create(
                request.Name,
                request.Description,
                request.ProductUseGuide,
                fileName,
                request.CategoryId
            );

            foreach (var variant in request.ProductVariants)
            {
                var productVariant = ProductVariant.Create(variant.VariantName, variant.Quantity);
                productVariant.SetPrice(variant.OriginalPrice);
                productVariant.ApplyDiscount(variant.DiscountPercent);
                product.AddProductVariants(productVariant);
            }
            return await repository.CreateAsync(product);
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
