using Product_API.Domain.Categories;
using Product_API.Features.Categories;
using Shared.Domain.Results;

namespace Product_API.Interfaces;

public interface ICategoryRepository
{
    Task<Result> CreateCategory(CreateCategory.Command command);
    Task<Result> DeleteCategory(DeleteCategory.Command command);
    Task<Result<Category>> GetCategoryById(GetCategoryById.Command command);
    Task<Result<List<Category>>> GetAllCategories();
}
