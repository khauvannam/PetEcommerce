using BaseDomain.Results;
using Microsoft.EntityFrameworkCore;
using Product_API.Databases;
using Product_API.Domains.Discounts;
using Product_API.Errors;
using Product_API.Interfaces;

namespace Product_API.Repositories;

public class DiscountRepository(ProductDbContext context) : IDiscountRepository
{
    public async Task<Result> CreateAsync(Discount discount)
    {
        context.Add(discount);
        await context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Discount discount)
    {
        context.Remove(discount);
        await context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateAsync(Discount discount)
    {
        context.Update(discount);
        await context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<Discount>> GetByIdAsync(Guid id)
    {
        var discount = await context.Discounts.FirstOrDefaultAsync(d => d.DiscountId == id);
        if (discount is null)
            return Result.Failure<Discount>(DiscountErrors.NotFound());

        return Result.Success(discount);
    }

    public async ValueTask<Result<List<Discount>>> GetAllAsync()
    {
        var discounts = await context.Discounts.AsNoTracking().ToListAsync();
        return Result.Success(discounts);
    }
}
