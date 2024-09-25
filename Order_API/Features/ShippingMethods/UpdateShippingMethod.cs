using BasedDomain.Results;
using MediatR;
using Order.API.Domains.ShippingMethods;
using Order.API.Interfaces;

namespace Order.API.Features.ShippingMethods;

public static class UpdateShippingMethod
{
    public record Command(string ShippingMethodId, string Name, decimal Price)
        : IRequest<Result<Shipping>>;

    internal sealed class Handler(IShippingMethodRepository shippingMethodRepository)
        : IRequestHandler<Command, Result<Shipping>>
    {
        public async Task<Result<Shipping>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var shippingMethodResult = await shippingMethodRepository.GetByIdAsync(
                request.ShippingMethodId
            );
            if (shippingMethodResult.IsFailure)
                return shippingMethodResult;

            var shippingMethod = shippingMethodResult.Value;

            shippingMethod.Update(request.Name);
            shippingMethod.SetPrice(request.Price);

            return await shippingMethodRepository.UpdateAsync(shippingMethod);
        }
    }
}
