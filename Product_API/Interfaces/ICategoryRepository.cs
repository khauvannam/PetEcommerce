using BaseDomain.Results;
using Product_API.Domains.Categories;
using Product_API.Features.Categories;

namespace Product_API.Interfaces;

public interface ICategoryRepository
{
    Task<Result> CreateCategory(Category category);
    Task<Result> DeleteCategory(Category category);
    Task<Result<Category>> UpdateCategory(Category category);
    ValueTask<Result<Category>> GetCategoryById(string categoryId);
    ValueTask<Result<List<Category>>> GetAllCategories();
}
