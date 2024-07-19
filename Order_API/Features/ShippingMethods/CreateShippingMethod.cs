using BaseDomain.Results;
using MediatR;
using Order.API.Domains.ShippingMethods;
using Order.API.Interfaces;

namespace Order.API.Features.ShippingMethods;

public static class CreateShippingMethod
{
    public record Command(string Name, decimal Price) : IRequest<Result<ShippingMethod>>;

    internal sealed class Handler(IShippingMethodRepository shippingMethodRepository)
        : IRequestHandler<Command, Result<ShippingMethod>>
    {
        public async Task<Result<ShippingMethod>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var shippingMethod = ShippingMethod.Create(request.Name);
            shippingMethod.SetPrice(request.Price);
            return await shippingMethodRepository.CreateAsync(shippingMethod);
        }
    }
}
