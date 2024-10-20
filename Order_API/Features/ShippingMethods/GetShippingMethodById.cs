using Base.Results;
using MediatR;
using Order.API.Domain.Shippings;
using Order.API.Interfaces;

namespace Order.API.Features.ShippingMethods;

public static class GetShippingMethodById
{
    public record Query(string ShippingMethodId) : IRequest<Result<Shipping>>;

    internal sealed class Handler(IShippingMethodRepository shippingMethodRepository)
        : IRequestHandler<Query, Result<Shipping>>
    {
        public async Task<Result<Shipping>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            return await shippingMethodRepository.GetByIdAsync(request.ShippingMethodId);
        }
    }
}
