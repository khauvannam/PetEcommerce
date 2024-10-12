using Base.Results;
using Basket_API.Interfaces;
using MediatR;

namespace Basket_API.Features.Baskets;

public static class DeleteBasket
{
    public record Command(Guid BasketId) : IRequest<Result>;

    internal sealed class Handler(IBasketRepository repository) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await repository.GetByIdAsync(request.BasketId);
            if (result.IsFailure)
                return result;

            var basket = result.Value;
            return await repository.DeleteAsync(basket);
        }
    }
}
