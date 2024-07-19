using BaseDomain.Results;
using Order.API.Databases;
using Order.API.Domains.ShippingMethods;
using Order.API.Errors;
using Order.API.Interfaces;

namespace Order.API.Repositories;

public class ShippingMethodRepository(OrderDbContext context) : IShippingMethodRepository
{
    public async Task<Result<ShippingMethod>> CreateAsync(ShippingMethod shippingMethod)
    {
        context.ShippingMethods.Add(shippingMethod);
        await context.SaveChangesAsync();
        return Result.Success(shippingMethod);
    }

    public async Task<Result<ShippingMethod>> UpdateAsync(ShippingMethod shippingMethod)
    {
        context.ShippingMethods.Update(shippingMethod);
        await context.SaveChangesAsync();
        return Result.Success(shippingMethod);
    }

    public async Task<Result> DeleteAsync(string shippingMethodId)
    {
        var shippingMethod = await context.ShippingMethods.FindAsync(shippingMethodId);
        if (shippingMethod != null)
        {
            context.ShippingMethods.Remove(shippingMethod);
            await context.SaveChangesAsync();
            return Result.Success();
        }
        return Result.Failure(ShippingMethodErrors.NotFound);
    }

    public async Task<Result<ShippingMethod>> GetByIdAsync(string shippingMethodId)
    {
        var shippingMethod = await context.ShippingMethods.FindAsync(shippingMethodId);
        if (shippingMethod != null)
        {
            return Result.Success(shippingMethod);
        }
        return Result.Failure<ShippingMethod>(ShippingMethodErrors.NotFound);
    }
}
