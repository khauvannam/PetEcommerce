using Basket_API.Domain.BasketItems;
using Basket_API.Interfaces;
using MediatR;
using Shared.Domain.Results;

namespace Basket_API.Features.BasketItems
{
    public static class GetBasketItemById
    {
        public record Query(string BasketItemId) : IRequest<Result<BasketItem>>;

        internal sealed class Handler(IBasketItemRepository repository)
            : IRequestHandler<Query, Result<BasketItem>>
        {
            public async Task<Result<BasketItem>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                return await repository.GetByIdAsync(request.BasketItemId);
            }
        }
    }
}
