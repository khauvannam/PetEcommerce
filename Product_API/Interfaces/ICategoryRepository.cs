using BaseDomain.Results;
using Product_API.Domains.Categories;
using Product_API.Features.Categories;

namespace Product_API.Interfaces;

public interface ICategoryRepository
{
    Task<Result> CreateCategory(CreateCategory.Command command);
    Task<Result> DeleteCategory(DeleteCategory.Command command);
    ValueTask<Result<Category>> GetCategoryById(GetCategoryById.Query command);
    ValueTask<Result<List<Category>>> GetAllCategories();
}
