using Base.Results;
using Product_API.Domain.Discounts;

namespace Product_API.Interfaces;

public interface IDiscountRepository
{
    public Task<Result> CreateAsync(Discount discount);
    public Task<Result> DeleteAsync(Discount discount);
    public Task<Result> UpdateAsync(Discount discount);
    public Task<Result<Discount>> GetByIdAsync(int id);
    public ValueTask<Result<List<Discount>>> GetAllAsync();
}
