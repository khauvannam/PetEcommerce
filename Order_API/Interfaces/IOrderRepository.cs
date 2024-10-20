using Base.Results;
using Order.API.Domain.Orders;

namespace Order.API.Interfaces;

public interface IOrderRepository
{
    Task<Result<OrderModel>> CreateAsync(OrderModel orderModel);

    Task<Result> DeleteAsync(OrderModel orderModel);
    Task<Result<OrderModel>> GetByIdAsync(int orderId);
}
