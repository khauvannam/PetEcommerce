using Basket_API.Domain.BasketItems;
using Basket_API.Domain.Baskets;
using Shared.Domain.Results;

namespace Basket_API.Interfaces;

public interface IBasketRepository
{
    Task<Result<Basket>> CreateAsync(Basket basket);
    Task<Result<Basket>> UpdateAsync(List<BasketItemRequest> basketItemRequests, Basket basket);
    Task<Result> DeleteAsync(string basketId);
    Task<Result<Basket>> GetByIdAsync(string basketId);
}
