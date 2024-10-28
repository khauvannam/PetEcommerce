using System.Reflection;
using Base;
using Base.Results;
using Microsoft.EntityFrameworkCore;
using Product_API.Databases;
using Product_API.Domain.Products;
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
        int productId,
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

    public async ValueTask<Result<Pagination<ListProductResponse>>> GetProductsBySearch(
        ProductsSearchFilterRequest filterRequest
    )
    {
        if (string.IsNullOrEmpty(filterRequest.SearchText))
        {
            return Result.Failure<Pagination<ListProductResponse>>(
                new("Search text is required", "Your search text is empty, please try again.")
            );
        }

        // Start with the base query for products
        var productsQueryable = dbContext
            .Products.FromSqlInterpolated(
                $"SELECT * FROM Products p WHERE LOWER(p.Name) LIKE {'%' + filterRequest.SearchText.Trim().ToLower() + '%'}"
            )
            .Include(p => p.ProductVariants)
            .AsQueryable();

        if (!filterRequest.Available)
        {
            productsQueryable = productsQueryable.Where(p => p.TotalQuantity == 0);
        }

        if (filterRequest.MinPrice > 0M)
        {
            productsQueryable = productsQueryable.Where(p =>
                p.ProductVariants.Any(v => v.OriginalPrice.Value >= filterRequest.MinPrice)
            );
        }

        if (filterRequest.MaxPrice is > 0M and < 10000M)
        {
            productsQueryable = productsQueryable.Where(p =>
                p.ProductVariants.Any(v => v.OriginalPrice.Value <= filterRequest.MaxPrice)
            );
        }

        if (!string.IsNullOrEmpty(filterRequest.FilterBy))
        {
            string sortByProperty;

            switch (filterRequest.FilterBy)
            {
                case "best-seller":
                    sortByProperty = nameof(Product.SoldQuantity);

                    productsQueryable = productsQueryable
                        .Where(p => p.SoldQuantity > 100)
                        .OrderByDescending(p => p.SoldQuantity);
                    break;
                case "new-arrivals":
                    sortByProperty = nameof(Product.CreatedAt);

                    var oneWeekAgo = DateTime.Now.AddDays(-7);
                    productsQueryable = productsQueryable
                        .Where(p => p.CreatedAt >= oneWeekAgo)
                        .OrderByDescending(p => p.CreatedAt);
                    break;
                case "high-rating":
                    sortByProperty = nameof(Product.TotalRating);

                    productsQueryable = productsQueryable
                        .Where(p => p.TotalRating > 3M)
                        .OrderByDescending(p => p.TotalRating);
                    break;
                default:
                    return Result.Failure<Pagination<ListProductResponse>>(
                        new(
                            "Invalid Sort Property",
                            $"Property '{filterRequest.FilterBy}' does not exist."
                        )
                    );
            }

            var propertyInfo = typeof(Product).GetProperty(sortByProperty)!;

            productsQueryable = filterRequest.IsDesc
                ? productsQueryable.OrderByDescending(p =>
                    EF.Property<object>(p, propertyInfo.Name)
                )
                : productsQueryable.OrderBy(p => EF.Property<object>(p, propertyInfo.Name));
        }

        // Apply pagination and project to response model
        var products = await productsQueryable
            .Skip(filterRequest.Offset)
            .Take(filterRequest.Limit)
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
            .ToListAsync();

        // Count total products after filters for pagination
        var totalCount = await productsQueryable.CountAsync();
        var pagination = new Pagination<ListProductResponse>(products, totalCount);

        return Result.Success(pagination);
    }

    public async Task<Result<ProductByIdResponse>> GetInDetailByIdAsync(
        int productId,
        CancellationToken cancellationToken
    )
    {
        var product = await dbContext
            .Products.Include(p => p.ProductVariants)
            .Include(p => p.Comments)
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductId == productId, cancellationToken);

        if (product is null)
        {
            return Result.Failure<ProductByIdResponse>(ProductErrors.NotFound);
        }

        var productResponse = Product.ToProductByIdResponse(product);

        return Result.Success(productResponse);
    }
}
