using BaseDomain.Results;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product_API.Domains.Products;
using Product_API.Interfaces;
using Shared.Errors;
using Shared.Services;

namespace Product_API.Features.Products;

public static class UpdateProduct
{
    public record Command(string ProductId, UpdateProductRequest UpdateProductRequest)
        : IRequest<Result<Product>>;

    public class Handler(IProductRepository repository, BlobService blobService)
        : IRequestHandler<Command, Result<Product>>
    {
        public async Task<Result<Product>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var result = await repository.GetByIdAsync(request.ProductId, cancellationToken);
            if (result.IsFailure)
            {
                return result;
            }

            var product = result.Value;
            var updateProductRequest = request.UpdateProductRequest;
            var fileName = await blobService.UploadFileAsync(updateProductRequest.File, "Product-");

            if (string.IsNullOrEmpty(fileName))
            {
                return Result.Failure<Product>(BlobErrors.ErrorUploadFile());
            }

            List<ProductVariant> variants = new();
            foreach (var variant in updateProductRequest.ProductVariants)
            {
                var productVariant = ProductVariant.Create(variant.VariantName, variant.Quantity);
                productVariant.SetPrice(variant.OriginalPrice);
                productVariant.ApplyDiscount(variant.DiscountPercent);
                variants.Add(productVariant);
            }

            product.UpdateProduct(
                updateProductRequest.Name,
                updateProductRequest.Description,
                updateProductRequest.ProductUseGuide,
                fileName,
                updateProductRequest.CategoryId,
                variants
            );

            return await repository.UpdateAsync(product, cancellationToken);
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
                        [FromForm] UpdateProductRequest updateProductRequest
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
                )
                .DisableAntiforgery();
        }
    }
}
