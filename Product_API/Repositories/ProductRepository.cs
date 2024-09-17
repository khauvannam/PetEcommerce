using BaseDomain.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Product_API.Databases;
using Product_API.Domains.Products;
using Product_API.Errors;
using Product_API.Features.Products;
using Product_API.Interfaces;

namespace Product_API.Repositories;

public class ProductRepository(ProductDbContext dbContext) : IProductRepository
{
    public async ValueTask<Result<List<Product>>> ListAllAsync(GetAllProducts.Query command)
    {
        var query = dbContext.Products.AsQueryable().AsNoTracking();

        if (command.CategoryId is not null)
        {
            query = query.Where(p => p.CategoryId == command.CategoryId);
        }

        if (command.Offset is not null && command.Limit is not null)
        {
            query = query.Skip((int)command.Offset!).Take((int)command.Limit!);
        }

        var products = await query.ToListAsync();

        return Result.Success(products);
    }

    public async Task<Result<Product>> CreateAsync(Product product)
    {
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();

        return Result.Success(product);
    }

    public async Task<Result> DeleteAsync(Product product, CancellationToken cancellationToken)
    {
        dbContext.Products.Remove(product);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<Product>> UpdateAsync(
        Product product,
        CancellationToken cancellationToken
    )
    {
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success(product);
    }

    public async Task<Result<Product>> GetByIdAsync(
        Guid productId,
        CancellationToken cancellationToken
    )
    {
        var product = await dbContext
            .Products.Include(p => p.ProductVariants)
            .FirstOrDefaultAsync(p => p.ProductId == productId, cancellationToken);

        return product == null
            ? Result.Failure<Product>(ProductErrors.NotFound)
            : Result.Success(product);
    }
}
