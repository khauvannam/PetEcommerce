using BasedDomain.Results;
using MediatR;
using Order.API.Interfaces;

namespace Order.API.Features.ShippingMethods;

public static class DeleteShippingMethod
{
    public record Command(string ShippingMethodId) : IRequest<Result>;

    internal sealed class Handler(IShippingMethodRepository repository)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await repository.GetByIdAsync(request.ShippingMethodId);
            if (result.IsFailure)
                return result;

            var shippingMethod = result.Value;
            return await repository.DeleteAsync(shippingMethod);
        }
    }
}
