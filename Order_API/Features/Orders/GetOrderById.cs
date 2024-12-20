using Base.Results;
using MediatR;
using Order.API.Domain.Orders;
using Order.API.Interfaces;

namespace Order.API.Features.Orders;

public class GetOrderById
{
    public record Query(int OrderId) : IRequest<Result<OrderModel>>;

    internal sealed class Handler(IOrderRepository orderRepository)
        : IRequestHandler<Query, Result<OrderModel>>
    {
        public async Task<Result<OrderModel>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            return await orderRepository.GetByIdAsync(request.OrderId);
        }
    }
}
