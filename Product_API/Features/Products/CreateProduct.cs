using BasedDomain.Results;
using FluentValidation;
using MediatR;
using Product_API.Domains.Products;
using Product_API.DTOs.Products;
using Product_API.Errors;
using Product_API.Interfaces;
using Product_API.Services;
using Shared.Errors;
using Shared.Services;

namespace Product_API.Features.Products;

public static class CreateProduct
{
    public sealed record Command(
        string Name,
        string Description,
        string ProductUseGuide,
        List<IFormFile> Images,
        int CategoryId,
        decimal Percent,
        List<ProductVariantRequest> ProductVariants
    ) : IRequest<Result<Product>>;

    public sealed class Handler(
        IProductRepository repository,
        BlobService blobService,
        ProductService service
    ) : IRequestHandler<Command, Result<Product>>
    {
        public async Task<Result<Product>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            if (service.CheckDuplicateProductName(request.Name))
            {
                return Result.Failure<Product>(ProductErrors.DuplicateName);
            }

            List<ProductVariant> productVariants = [];
            List<string> imageUrlList = [];

            foreach (var image in request.Images)
            {
                var fileName = await blobService.UploadFileAsync(image, "Products-");
                imageUrlList.Add(fileName);
            }

            var product = Product.Create(
                request.Name,
                request.Description,
                request.ProductUseGuide,
                request.CategoryId
            );

            foreach (var variant in request.ProductVariants)
            {
                var productVariant = ProductVariant.Create(variant.VariantName, variant.Quantity);
                productVariant.SetPrice(variant.OriginalPrice);
                productVariants.Add(productVariant);
            }

            product.AddProductVariants(productVariants);
            product.AddImageUrl(imageUrlList);
            product.CalculateTotalQuantity();
            product.UpdatePrice();
            product.ApplyDiscount(request.Percent);

            return await repository.CreateAsync(product);
        }
    }

    public class Validator : AbstractValidator<Product>;
}
