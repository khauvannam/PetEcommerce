using Base;
using Base.Results;
using Microsoft.EntityFrameworkCore;
using Product_API.Databases;
using Product_API.Domains.Products;
using Product_API.DTOs.Products;
using Product_API.Errors;
using Product_API.Features.Products;
using Product_API.Interfaces;

namespace Product_API.Repositories;

public class ProductRepository(ProductDbContext dbContext) : IProductRepository
{
    public async ValueTask<Result<Pagination<ListProductResponse>>> ListAllAsync(
        GetAllProducts.Query query
    )
    {
        var queryable = dbContext.Products.AsQueryable().AsNoTracking();

        if (query.CategoryId is not null)
        {
            queryable = queryable.Where(p => p.CategoryId == query.CategoryId);
        }

        if (query.IsBestSeller)
        {
            queryable = queryable
                .Where(p => p.SoldQuantity > 100)
                .OrderByDescending(p => p.SoldQuantity);
        }

        var products = await queryable
            .Select(p => new ListProductResponse
            {
                ProductId = p.ProductId,
                CreatedAt = p.CreatedAt,
                Description = p.Description,
                DiscountPercent = p.DiscountPercent.Value,
                ImageUrl = p.ImageUrlList[0],
                Name = p.Name,
                Price = p.ProductVariants[0].OriginalPrice.Value,
                Quantity = p.TotalQuantity,
                TotalRating = p.TotalRating,
                SoldQuantity = p.SoldQuantity,
            })
            .Skip(query.Offset)
            .Take(query.Limit)
            .ToListAsync();

        var pagination = new Pagination<ListProductResponse>(products, queryable.Count());

        return Result.Success(pagination);
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
        var product = await dbContext.Products.FirstOrDefaultAsync(
            p => p.ProductId == productId,
            cancellationToken
        );

        return product == null
            ? Result.Failure<Product>(ProductErrors.NotFound)
            : Result.Success(product);
    }

    public async Task<Result<ProductByIdResponse>> GetInDetailByIdAsync(
        Guid productId,
        CancellationToken cancellationToken
    )
    {
        var product = await dbContext
            .Products.Include(p => p.ProductVariants)
            .Include(p => p.Comments)
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductId == productId, cancellationToken);

        var productResponse = Product.ToProductByIdResponse(product!);

        return product == null
            ? Result.Failure<ProductByIdResponse>(ProductErrors.NotFound)
            : Result.Success(productResponse);
    }
}
