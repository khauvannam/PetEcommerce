using BasedDomain;
using BasedDomain.Results;
using Product_API.Domains.Categories;
using Product_API.Features.Categories;

namespace Product_API.Interfaces;

public interface ICategoryRepository
{
    Task<Result> CreateAsync(Category category);
    Task<Result> DeleteAsync(Category category);
    Task<Result<Category>> UpdateAsync(Category category);
    ValueTask<Result<Category>> GetByIdAsync(int categoryId);
    Task<Result<Category>> GetByEndpointAsync(GetCategoryByEndpoint.Query query);
    ValueTask<Result<Pagination<Category>>> GetAllAsync();
}
