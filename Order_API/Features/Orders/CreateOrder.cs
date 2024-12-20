using Base.Results;
using MediatR;
using Order.API.Domain.OrderLines;
using Order.API.Domain.Orders;
using Order.API.Interfaces;

namespace Order.API.Features.Orders;

public static class CreateOrder
{
    public record Command(int UserId, string ShippingAddress, List<OrderLine> OrderLines)
        : IRequest<Result<OrderModel>>;

    internal sealed class Handler(IOrderRepository orderRepository)
        : IRequestHandler<Command, Result<OrderModel>>
    {
        public async Task<Result<OrderModel>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var order = OrderModel.Create(request.UserId, request.ShippingAddress);
            foreach (var orderLine in request.OrderLines)
                order.AddOrderLine(orderLine);

            return await orderRepository.CreateAsync(order);
        }
    }
}
