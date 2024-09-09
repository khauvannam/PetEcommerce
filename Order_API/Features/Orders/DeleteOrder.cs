using BaseDomain.Results;
using MediatR;
using Order.API.Interfaces;

namespace Order.API.Features.Orders;

public class DeleteOrder
{
    public record Command(string OrderId) : IRequest<Result>;

    internal sealed class Handler(IOrderRepository orderRepository)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await orderRepository.GetByIdAsync(request.OrderId);
            if (result.IsFailure)
                return result;
            return await orderRepository.DeleteAsync(result.Value);
        }
    }
}
