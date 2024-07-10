using Basket_API.Domain.BasketItems;
using Basket_API.Domain.Baskets;
using Basket_API.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Shared.Domain.Results;
using Shared.Domain.Services;

namespace Basket_API.Repositories;

public class CachedBasketRepository(IDistributedCache cache, IBasketRepository decorated)
    : IBasketRepository
{
    public async Task<Result<Basket>> CreateAsync(Basket basket)
    {
        var key = $"basket-{basket.BasketId}";
        var result = await decorated.CreateAsync(basket);
        if (result.IsFailure)
        {
            return result;
        }
        await cache.SetStringAsync(key, JsonConvert.SerializeObject(basket));
        return result;
    }

    public async Task<Result<Basket>> UpdateAsync(
        List<BasketItemRequest> basketItemRequests,
        Basket basket
    )
    {
        var key = $"basket-{basket.BasketId}";

        await cache.SetStringAsync(key, JsonConvert.SerializeObject(basket));

        var result = await decorated.UpdateAsync(basketItemRequests, basket);
        return result;
    }

    public async Task<Result> DeleteAsync(string basketId)
    {
        var key = $"basket-{basketId}";
        var result = await decorated.DeleteAsync(basketId);
        var cachedBasket = await cache.GetStringAsync(key);
        if (!string.IsNullOrEmpty(cachedBasket))
        {
            await cache.RemoveAsync(key);
        }

        return result;
    }

    public async Task<Result<Basket>> GetByIdAsync(string basketId)
    {
        var key = $"basket-{basketId}";
        var cachedBasket = await cache.GetStringAsync(key);
        if (string.IsNullOrEmpty(cachedBasket))
        {
            var result = await decorated.GetByIdAsync(basketId);
            if (result.IsFailure)
                return result;

            await cache.SetStringAsync(key, JsonConvert.SerializeObject(result.Value));
            return result;
        }

        var basket = JsonConvert.DeserializeObject<Basket>(
            cachedBasket,
            new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver = new PrivateSetterJsonResolver()
            }
        )!;
        return Result.Success(basket);
    }
}
