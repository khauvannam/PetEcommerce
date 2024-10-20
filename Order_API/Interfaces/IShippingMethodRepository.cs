using Base.Results;
using Order.API.Domain.Shippings;

namespace Order.API.Interfaces;

public interface IShippingMethodRepository
{
    Task<Result<Shipping>> CreateAsync(Shipping shipping);
    Task<Result<Shipping>> UpdateAsync(Shipping shipping);
    Task<Result> DeleteAsync(Shipping shipping);
    Task<Result<Shipping>> GetByIdAsync(string shippingMethodId);
}
