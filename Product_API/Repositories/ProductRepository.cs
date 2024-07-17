using BaseDomain.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Product_API.Databases;
using Product_API.Domains.Products;
using Product_API.Errors;
using Product_API.Features.Products;
using Product_API.Interfaces;
using Shared.Errors;
using Shared.Services;

namespace Product_API.Repositories
{
    public class ProductRepository(ProductDbContext dbContext, BlobService blobService)
        : IProductRepository
    {
        public async ValueTask<Result<List<Product>>> ListAllProducts(ListAllProducts.Query command)
        {
            var query = dbContext.Products.AsQueryable();

            if (!command.CategoryId.IsNullOrEmpty())
            {
                query = query.Where(p => p.ProductCategory.ProductCategoryId == command.CategoryId);
            }

            var products = await query.Skip(command.Offset).Take(command.Limit).ToListAsync();

            return Result.Success(products);
        }

        public async Task<Result<Product>> CreateProduct(CreateProduct.Command command)
        {
            var productCategory = ProductCategory.Create(
                command.ProductCategory.ProductCategoryId,
                command.ProductCategory.Details
            );
            var fileName = (await blobService.UploadFileAsync(command.File, "Product-")).Value;

            if (string.IsNullOrEmpty(fileName))
            {
                return Result.Failure<Product>(BlobErrors.ErrorUploadFile());
            }

            var product = Product.Create(
                command.Name,
                command.Description,
                command.ProductUseGuide,
                fileName,
                productCategory
            );

            foreach (var variant in command.ProductVariants)
            {
                var productVariant = ProductVariant.Create(variant.VariantName, variant.Quantity);
                productVariant.SetPrice(variant.OriginalPrice);
                productVariant.ApplyDiscount(variant.DiscountPercent);
                product.AddProductVariants(productVariant);
            }

            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();

            return Result.Success(product);
        }

        public async Task<Result> DeleteProduct(
            string productId,
            CancellationToken cancellationToken
        )
        {
            var product = await dbContext
                .Products.Include(p => p.ProductVariants)
                .FirstOrDefaultAsync(
                    p => p.ProductId == productId,
                    cancellationToken: cancellationToken
                );

            if (product == null)
            {
                return Result.Failure(ProductErrors.NotFound);
            }

            var fileName = new Uri(product.ImageUrl).Segments[^1];
            dbContext.Products.Remove(product);
            await blobService.DeleteAsync(fileName);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<Product>> UpdateProduct(
            UpdateProduct.Command command,
            CancellationToken cancellationToken
        )
        {
            var product = await dbContext
                .Products.Include(p => p.ProductVariants)
                .FirstOrDefaultAsync(
                    p => p.ProductId == command.ProductId,
                    cancellationToken: cancellationToken
                );

            if (product == null)
            {
                return Result.Failure<Product>(ProductErrors.NotFound);
            }

            var updateProductRequest = command.UpdateProductRequest;
            var fileName = (
                await blobService.UploadFileAsync(updateProductRequest.File, "Product-")
            ).Value;

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
                updateProductRequest.ProductCategory,
                variants
            );

            await dbContext.SaveChangesAsync(cancellationToken);
            return Result.Success(product);
        }

        public async Task<Result<Product>> GetProductById(
            string productId,
            CancellationToken cancellationToken
        )
        {
            var product = await dbContext
                .Products.Include(p => p.ProductVariants)
                .FirstOrDefaultAsync(
                    p => p.ProductId == productId,
                    cancellationToken: cancellationToken
                );

            return product == null
                ? Result.Failure<Product>(ProductErrors.NotFound)
                : Result.Success(product);
        }
    }
}
