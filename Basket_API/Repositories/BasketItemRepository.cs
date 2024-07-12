using Basket_API.Database;
using Basket_API.Domains.BasketItems;
using Basket_API.Errors;
using Basket_API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Results;

namespace Basket_API.Repositories
{
    public class BasketItemRepository(BasketDbContext context) : IBasketItemRepository
    {
        public async Task<Result<BasketItem>> CreateAsync(BasketItem basketItem)
        {
            context.BasketItems.Add(basketItem);
            await context.SaveChangesAsync();
            return Result.Success(basketItem);
        }

        public async Task<Result<BasketItem>> UpdateAsync(BasketItem basketItem)
        {
            context.BasketItems.Update(basketItem);
            await context.SaveChangesAsync();
            return Result.Success(basketItem);
        }

        public async Task<Result> DeleteAsync(string basketItemId)
        {
            var basketItem = await context.BasketItems.FirstOrDefaultAsync(bi =>
                bi.BasketItemId == basketItemId
            );
            if (basketItem == null)
                return Result.Failure(BasketItemErrors.NotFound);
            context.BasketItems.Remove(basketItem);
            await context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<BasketItem>> GetByIdAsync(string basketItemId)
        {
            var basketItem = await context.BasketItems.FirstOrDefaultAsync(bi =>
                bi.BasketItemId == basketItemId
            );
            if (basketItem != null)
            {
                return Result.Success(basketItem);
            }
            return Result.Failure<BasketItem>(BasketItemErrors.NotFound);
        }
    }
}
