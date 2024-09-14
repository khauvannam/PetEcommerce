using BaseDomain.Results;
using Order.API.Databases;
using Order.API.Domains.ShippingMethods;
using Order.API.Errors;
using Order.API.Interfaces;

namespace Order.API.Repositories;

public class ShippingMethodRepository(OrderDbContext context) : IShippingMethodRepository
{
    public async Task<Result<Shipping>> CreateAsync(Shipping shipping)
    {
        context.ShippingMethods.Add(shipping);
        await context.SaveChangesAsync();
        return Result.Success(shipping);
    }

    public async Task<Result<Shipping>> UpdateAsync(Shipping shipping)
    {
        context.ShippingMethods.Update(shipping);
        await context.SaveChangesAsync();
        return Result.Success(shipping);
    }

    public async Task<Result> DeleteAsync(Shipping shipping)
    {
        context.ShippingMethods.Remove(shipping);
        await context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<Shipping>> GetByIdAsync(string shippingMethodId)
    {
        var shippingMethod = await context.ShippingMethods.FindAsync(shippingMethodId);
        if (shippingMethod != null)
            return Result.Success(shippingMethod);
        return Result.Failure<Shipping>(ShippingMethodErrors.NotFound);
    }
}
