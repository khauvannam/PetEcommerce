using BaseDomain.Results;
using MediatR;
using Product_API.Domains.Products;
using Product_API.Interfaces;
using Shared.Errors;
using Shared.Services;

namespace Product_API.Features.Products;

public static class UpdateProduct
{
    public sealed record Command(Guid ProductId, UpdateProductRequest UpdateProductRequest)
        : IRequest<Result<Product>>;

    public sealed class Handler(IProductRepository repository, BlobService blobService)
        : IRequestHandler<Command, Result<Product>>
    {
        public async Task<Result<Product>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var result = await repository.GetByIdAsync(request.ProductId, cancellationToken);
            if (result.IsFailure)
                return result;

            var product = result.Value;
            var updateProductRequest = request.UpdateProductRequest;
            var newFileName = string.Empty;

            if (updateProductRequest.File is not null)
            {
                var fileName = new Uri(product.ImageUrl).Segments.Last();
                await blobService.DeleteAsync(fileName);

                newFileName = await blobService.UploadFileAsync(
                    updateProductRequest.File,
                    "Product-"
                );
            }

            if (string.IsNullOrEmpty(newFileName))
                return Result.Failure<Product>(BlobErrors.ErrorUploadFile());

            List<ProductVariant> variants = [];
            foreach (var variant in updateProductRequest.ProductVariants)
            {
                var productVariant = ProductVariant.Create(variant.VariantName, variant.Quantity);
                productVariant.SetPrice(variant.OriginalPrice);
                variants.Add(productVariant);
            }

            product.UpdateProduct(
                updateProductRequest.Name,
                updateProductRequest.Description,
                updateProductRequest.ProductUseGuide,
                newFileName,
                updateProductRequest.CategoryId,
                variants
            );
            product.ApplyDiscount(updateProductRequest.Percent);

            return await repository.UpdateAsync(product, cancellationToken);
        }
    }
}
