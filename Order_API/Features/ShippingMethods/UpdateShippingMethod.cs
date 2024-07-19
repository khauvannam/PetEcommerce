using BaseDomain.Results;
using MediatR;
using Order.API.Domains.ShippingMethods;
using Order.API.Errors;
using Order.API.Interfaces;

namespace Order.API.Features.ShippingMethods;

public static class UpdateShippingMethod
{
    public record Command(string ShippingMethodId, string Name, decimal Price)
        : IRequest<Result<ShippingMethod>>;

    internal sealed class Handler(IShippingMethodRepository shippingMethodRepository)
        : IRequestHandler<Command, Result<ShippingMethod>>
    {
        public async Task<Result<ShippingMethod>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var shippingMethodResult = await shippingMethodRepository.GetByIdAsync(
                request.ShippingMethodId
            );
            if (shippingMethodResult.IsFailure)
            {
                return Result.Failure<ShippingMethod>(ShippingMethodErrors.NotFound);
            }

            var shippingMethod = shippingMethodResult.Value!;

            shippingMethod.Update(request.Name);
            shippingMethod.SetPrice(request.Price);

            return await shippingMethodRepository.UpdateAsync(shippingMethod);
        }
    }
}
