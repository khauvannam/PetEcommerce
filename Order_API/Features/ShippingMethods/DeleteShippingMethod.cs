using BaseDomain.Results;
using MediatR;
using Order.API.Interfaces;

namespace Order.API.Features.ShippingMethods;

public static class DeleteShippingMethod
{
    public record Command(string ShippingMethodId) : IRequest<Result>;

    internal sealed class Handler(IShippingMethodRepository shippingMethodRepository)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            return await shippingMethodRepository.DeleteAsync(request.ShippingMethodId);
        }
    }
}
