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
        public async Task<Result> CreateCategory(CreateCategory.Command command)
        {
            var fileName = (await blobService.UploadFileAsync(command.File, "Category-")).Value;
            var category = Category.Create(command.CategoryName, command.Description, fileName);

            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> DeleteCategory(DeleteCategory.Command command)
        {
            var category = dbContext.Categories.FirstOrDefault(c =>
                c.CategoryId == command.CategoryId
            );

            if (category == null)
            {
                return Result.Failure(CategoryErrors.NotFound);
            }
            var fileName = new Uri(category.ImageUrl).Segments[^1];
            dbContext.Categories.Remove(category);
            await blobService.DeleteAsync(fileName);
            await dbContext.SaveChangesAsync();

            return Result.Success();
        }

        public async ValueTask<Result<Category>> GetCategoryById(GetCategoryById.Query command)
        {
            var category = await dbContext.Categories.FirstOrDefaultAsync(c =>
                c.CategoryId == command.CategoryId
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
