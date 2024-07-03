using Basket_API.Interfaces;
using MediatR;
using Shared.Domain.Results;

namespace Basket_API.Features.BasketItems
{
    public static class DeleteBasketItem
    {
        public record Command(string BasketItemId) : IRequest<Result>;

        internal sealed class Handler(IBasketItemRepository repository)
            : IRequestHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                return await repository.DeleteAsync(request.BasketItemId);
            }
        }
    }
}
