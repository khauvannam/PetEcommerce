using Base.Results;
using Basket_API.Domain.Baskets;

namespace Basket_API.Interfaces;

public interface IBasketRepository
{
    Task<Result<Basket>> CreateAsync(Basket basket);
    Task<Result<Basket>> UpdateAsync(Basket basket);
    Task<Result> DeleteAsync(Basket basket);
    Task<Result<Basket>> GetByIdAsync(int basketId);
}
