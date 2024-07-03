using Basket_API.Database;
using Basket_API.Domain.Baskets;
using Basket_API.Errors;
using Basket_API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Results;

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
        context.Baskets.Update(basket);
        await context.SaveChangesAsync();
        return Result.Success(basket);
    }

    public async Task<Result> DeleteAsync(string basketId)
    {
        var basket = await context
            .Baskets.Include(b => b.BasketItemsList)
            .FirstOrDefaultAsync(b => b.BasketId == basketId);
        if (basket != null)
        {
            context.Baskets.Remove(basket);
            await context.SaveChangesAsync();
            return Result.Success();
        }
        return Result.Failure(BasketErrors.NotFound);
    }

    public async Task<Result<Basket>> GetByIdAsync(string basketId)
    {
        var basket = await context
            .Baskets.Include(b => b.BasketItemsList)
            .FirstOrDefaultAsync(b => b.BasketId == basketId);
        if (basket != null)
        {
            return Result.Success(basket);
        }
        return Result.Failure<Basket>(BasketErrors.NotFound);
    }
}
