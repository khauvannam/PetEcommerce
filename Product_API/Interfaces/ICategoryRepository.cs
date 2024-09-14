using BaseDomain.Results;
using Product_API.Domains.Categories;

namespace Product_API.Interfaces;

public interface ICategoryRepository
{
    Task<Result> CreateAsync(Category category);
    Task<Result> DeleteAsync(Category category);
    Task<Result<Category>> UpdateAsync(Category category);
    ValueTask<Result<Category>> GetByIdAsync(Guid categoryId);
    ValueTask<Result<List<Category>>> GetAllAsync();
}
