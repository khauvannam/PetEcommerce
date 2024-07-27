using BaseDomain.Results;
using Microsoft.EntityFrameworkCore;
using Product_API.Databases;
using Product_API.Domains.Categories;
using Product_API.Errors;
using Product_API.Features.Categories;
using Product_API.Interfaces;
using Shared.Services;

namespace Product_API.Repositories
{
    public class CategoryRepository(ProductDbContext dbContext, BlobService blobService)
        : ICategoryRepository
    {
        public async Task<Result> CreateCategory(Category category)
        {
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> DeleteCategory(Category category)
        {
            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<Category>> UpdateCategory(Category category)
        {
            await dbContext.SaveChangesAsync();
            return Result.Success(category);
        }

        public async ValueTask<Result<Category>> GetCategoryById(string categoryId)
        {
            var category = await dbContext.Categories.FirstOrDefaultAsync(c =>
                c.CategoryId == categoryId
            );

            return category == null
                ? Result.Failure<Category>(CategoryErrors.NotFound)
                : Result.Success(category);
        }

        public async ValueTask<Result<List<Category>>> GetAllCategories()
        {
            var categories = await dbContext.Categories.AsQueryable().ToListAsync();
            return Result.Success(categories);
        }
    }
}
