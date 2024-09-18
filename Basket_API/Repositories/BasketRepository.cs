using BaseDomain.Results;
using Basket_API.Database;
using Basket_API.Domains.Baskets;
using Basket_API.Errors;
using Basket_API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Basket_API.Repositories;

public class BasketRepository(BasketDbContext context) : IBasketRepository
{
    public async Task<Result<Basket>> CreateAsync(Basket basket)
    {
        context.Baskets.Add(basket);
        await context.SaveChangesAsync();
        return Result.Success(basket);
    }

    public async Task<Result<Basket>> UpdateAsync(Basket basket)
    {
        await context.SaveChangesAsync();
        return Result.Success(basket);
    }

    public async Task<Result> DeleteAsync(Basket basket)
    {
        context.Baskets.Remove(basket);
        await context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<Basket>> GetByIdAsync(Guid basketId)
    {
        var basket = await context
            .Baskets.Include(b => b.BasketItemsList)
            .AsSplitQuery()
            .FirstOrDefaultAsync(b => b.BasketId == basketId);
        if (basket != null)
            return Result.Success(basket);
        return Result.Failure<Basket>(BasketErrors.NotFound);
    }
}
