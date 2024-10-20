using Base.Results;
using Basket_API.Domain.BasketItems;

namespace Basket_API.Interfaces;

public interface IBasketItemRepository
{
    Task<Result<BasketItem>> CreateAsync(BasketItem basketItem);
    Task<Result<BasketItem>> UpdateAsync(BasketItem basketItem);
    Task<Result> DeleteAsync(int basketItemId);
    Task<Result<BasketItem>> GetByIdAsync(int basketItemId);
}
