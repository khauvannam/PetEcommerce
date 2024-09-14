using BaseDomain.Results;
using FluentValidation;
using MediatR;
using Product_API.Domains.Products;
using Product_API.Errors;
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
        Guid CategoryId,
        decimal Percent,
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
            var products = (
                await repository.ListAllAsync(new GetAllProducts.Query(null, null))
            ).Value.Select(p => p.Name);

            if (products.Contains(request.Name))
            {
                return Result.Failure<Product>(ProductErrors.DuplicateName);
            }

            var fileName = await blobService.UploadFileAsync(request.File, "Product-");

            if (string.IsNullOrEmpty(fileName))
                return Result.Failure<Product>(BlobErrors.ErrorUploadFile());

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
                product.AddProductVariants(productVariant);
            }

            product.ApplyDiscount(request.Percent);

            return await repository.CreateAsync(product);
        }
    }

    public class Validator : AbstractValidator<Product>;
}
