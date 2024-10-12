using Base.Results;
using MediatR;
using Order.API.Domains.ShippingMethods;
using Order.API.Interfaces;

namespace Order.API.Features.ShippingMethods;

public static class CreateShippingMethod
{
    public record Command(string Name, decimal Price) : IRequest<Result<Shipping>>;

    internal sealed class Handler(IShippingMethodRepository shippingMethodRepository)
        : IRequestHandler<Command, Result<Shipping>>
    {
        public async Task<Result<Shipping>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var shippingMethod = Shipping.Create(request.Name);
            shippingMethod.SetPrice(request.Price);
            return await shippingMethodRepository.CreateAsync(shippingMethod);
        }
    }
}
