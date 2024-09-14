using BaseDomain.Results;
using Basket_API.Domains.BasketItems;

namespace Basket_API.Interfaces;

public interface IBasketItemRepository
{
    Task<Result<BasketItem>> CreateAsync(BasketItem basketItem);
    Task<Result<BasketItem>> UpdateAsync(BasketItem basketItem);
    Task<Result> DeleteAsync(Guid basketItemId);
    Task<Result<BasketItem>> GetByIdAsync(Guid basketItemId);
}
