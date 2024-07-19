using BaseDomain.Results;
using Order.API.Domains.ShippingMethods;

namespace Order.API.Interfaces;

public interface IShippingMethodRepository
{
    Task<Result<ShippingMethod>> CreateAsync(ShippingMethod shippingMethod);
    Task<Result<ShippingMethod>> UpdateAsync(ShippingMethod shippingMethod);
    Task<Result> DeleteAsync(string shippingMethodId);
    Task<Result<ShippingMethod>> GetByIdAsync(string shippingMethodId);
}
