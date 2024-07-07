using Basket_API.Domain.BasketItems;
using Basket_API.Domain.Baskets;
using Basket_API.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Domain.Results;

namespace Basket_API.Repositories;

public class CachedBasketRepository(IDistributedCache cache, IBasketRepository decorated)
    : IBasketRepository
{
    public Task<Result<Basket>> CreateAsync(Basket basket)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Basket>> UpdateAsync(
        List<BasketItemRequest> basketItemRequests,
        Basket basket
    )
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(string basketId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Basket>> GetByIdAsync(string basketId)
    {
        throw new NotImplementedException();
    }
}
