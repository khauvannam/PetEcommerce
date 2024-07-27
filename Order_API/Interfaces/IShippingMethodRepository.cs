using BaseDomain.Results;
using Order.API.Domains.ShippingMethods;

namespace Order.API.Interfaces;

public interface IShippingMethodRepository
{
    Task<Result<ShippingMethod>> CreateAsync(ShippingMethod shippingMethod);
    Task<Result<ShippingMethod>> UpdateAsync(ShippingMethod shippingMethod);
    Task<Result> DeleteAsync(ShippingMethod shippingMethod);
    Task<Result<ShippingMethod>> GetByIdAsync(string shippingMethodId);
}
