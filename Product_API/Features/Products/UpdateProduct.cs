using Base.Results;
using MediatR;
using Product_API.Domains.Products;
using Product_API.DTOs.Products;
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
            List<ProductVariant> variants = [];

            List<string> updatedImageUrls = [];

            var existingFileNames = product
                .ImageUrlList.Select(n => new Uri(n).Segments.Last())
                .ToHashSet();

            // Process the images in the request, keeping or uploading new ones
            foreach (var image in updateProductRequest.Images)
            {
                // Check if the image already exists by its name
                if (existingFileNames.Contains(image.Name))
                {
                    // Keep the image, maintain its original URL from the product's list
                    var existingImageUrl = product.ImageUrlList.First(url =>
                        new Uri(url).Segments.Last() == image.Name
                    );
                    updatedImageUrls.Add(existingImageUrl);
                }
                else
                {
                    // Upload the new image and add its URL
                    var uploadedImageUrl = await blobService.UploadFileAsync(image, "Products-");
                    updatedImageUrls.Add(uploadedImageUrl);
                }
            }

            // Delete images that are no longer in the request
            foreach (var fileName in product.ImageUrlList.Select(n => new Uri(n).Segments.Last()))
            {
                if (!updateProductRequest.Images.Select(variant => variant.Name).Contains(fileName))
                {
                    await blobService.DeleteAsync(fileName);
                }
            }

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
                updateProductRequest.CategoryId,
                variants
            );

            product.ApplyDiscount(updateProductRequest.Percent);
            product.CalculateTotalQuantity();
            product.AddImageUrl(updatedImageUrls);

            return await repository.UpdateAsync(product, cancellationToken);
        }
    }
}
