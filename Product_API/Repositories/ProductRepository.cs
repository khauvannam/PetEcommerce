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
        public async ValueTask<Result<List<Product>>> ListAllProducts(GetAllProducts.Query command)
        {
            var query = dbContext.Products.AsQueryable();

            if (!command.CategoryId.IsNullOrEmpty())
            {
                query = query.Where(p => p.CategoryId == command.CategoryId);
            }

            var products = await query.Skip(command.Offset).Take(command.Limit).ToListAsync();

            return Result.Success(products);
        }

        public async Task<Result<Product>> CreateProduct(Product product)
        {
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();

            return Result.Success(product);
        }

        public async Task<Result> DeleteProduct(
            Product product,
            CancellationToken cancellationToken
        )
        {
            dbContext.Products.Remove(product);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<Product>> UpdateProduct(
            Product product,
            CancellationToken cancellationToken
        )
        {
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
