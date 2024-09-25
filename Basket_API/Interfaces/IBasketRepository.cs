using BasedDomain.Results;
using Basket_API.Domains.Baskets;

namespace Basket_API.Interfaces;

public interface IBasketRepository
{
    Task<Result<Basket>> CreateAsync(Basket basket);
    Task<Result<Basket>> UpdateAsync(Basket basket);
    Task<Result> DeleteAsync(Basket basket);
    Task<Result<Basket>> GetByIdAsync(Guid basketId);
}
