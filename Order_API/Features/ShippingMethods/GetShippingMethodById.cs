using BaseDomain.Results;
using MediatR;
using Order.API.Domains.ShippingMethods;
using Order.API.Interfaces;

namespace Order.API.Features.ShippingMethods;

public static class GetShippingMethodById
{
    public record Query(string ShippingMethodId) : IRequest<Result<ShippingMethod>>;

    internal sealed class Handler(IShippingMethodRepository shippingMethodRepository)
        : IRequestHandler<Query, Result<ShippingMethod>>
    {
        public async Task<Result<ShippingMethod>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            return await shippingMethodRepository.GetByIdAsync(request.ShippingMethodId);
        }
    }
}
