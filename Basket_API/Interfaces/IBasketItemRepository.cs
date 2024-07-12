using Basket_API.Domains.BasketItems;
using Shared.Domain.Results;

namespace Basket_API.Interfaces;

public interface IBasketItemRepository
{
    Task<Result<BasketItem>> CreateAsync(BasketItem basketItem);
    Task<Result<BasketItem>> UpdateAsync(BasketItem basketItem);
    Task<Result> DeleteAsync(string basketItemId);
    Task<Result<BasketItem>> GetByIdAsync(string basketItemId);
}
