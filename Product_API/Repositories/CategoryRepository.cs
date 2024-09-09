using BaseDomain.Results;
using Microsoft.EntityFrameworkCore;
using Product_API.Databases;
using Product_API.Domains.Categories;
using Product_API.Errors;
using Product_API.Interfaces;

namespace Product_API.Repositories;

public class CategoryRepository(ProductDbContext dbContext) : ICategoryRepository
{
    public async Task<Result> CreateAsync(Category category)
    {
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Category category)
    {
        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<Category>> UpdateAsync(Category category)
    {
        await dbContext.SaveChangesAsync();
        return Result.Success(category);
    }

    public async ValueTask<Result<Category>> GetByIdAsync(string categoryId)
    {
        var category = await dbContext.Categories.FirstOrDefaultAsync(c =>
            c.CategoryId == categoryId
        );

        return category == null
            ? Result.Failure<Category>(CategoryErrors.NotFound)
            : Result.Success(category);
    }

    public async ValueTask<Result<List<Category>>> GetAllAsync()
    {
        var categories = await dbContext.Categories.AsNoTracking().ToListAsync();
        return Result.Success(categories);
    }
}
