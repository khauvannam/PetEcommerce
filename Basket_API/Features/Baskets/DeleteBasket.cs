using Basket_API.Interfaces;
using MediatR;
using Shared.Domain.Results;

namespace Basket_API.Features.Baskets
{
    public static class DeleteBasket
    {
        public record Command(string BasketId) : IRequest<Result>;

        internal sealed class Handler(IBasketRepository repository)
            : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                return await repository.DeleteAsync(request.BasketId);
            }
        }
    }
}
