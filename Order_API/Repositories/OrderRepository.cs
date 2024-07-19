using BaseDomain.Results;
using Microsoft.EntityFrameworkCore;
using Order.API.Databases;
using Order.API.Domains.Orders;
using Order.API.Errors;
using Order.API.Interfaces;

namespace Order.API.Repositories;

public class OrderRepository(OrderDbContext context) : IOrderRepository
{
    public async Task<Result<OrderModel>> CreateAsync(OrderModel order)
    {
        context.Orders.Add(order);
        await context.SaveChangesAsync();
        return Result.Success(order);
    }

    public async Task<Result> DeleteAsync(string orderId)
    {
        var order = await context
            .Orders.Include(o => o.OrderLines)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);

        if (order == null)
        {
            return Result.Failure(OrderErrors.NotFound);
        }

        context.Orders.Remove(order);
        await context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<OrderModel>> GetByIdAsync(string orderId)
    {
        var order = await context
            .Orders.Include(o => o.OrderLines)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);

        if (order == null)
        {
            return Result.Failure<OrderModel>(OrderErrors.NotFound);
        }

        return Result.Success(order);
    }
}
