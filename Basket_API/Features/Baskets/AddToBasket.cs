using Base.Results;
using Basket_API.Domain.Baskets;
using Basket_API.DTOs.BasketItems;
using Basket_API.Interfaces;
using MediatR;

namespace Basket_API.Features.Baskets;

public static class AddToBasket
{
    public record Command(int CustomerId, UpdateBasketItemRequest UpdateBasketItemRequest)
        : IRequest<Result<Basket>>;

    public sealed class Handler(IBasketRepository basketRepository)
        : IRequestHandler<Command, Result<Basket>>
    {
        public async Task<Result<Basket>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var basketResult = await basketRepository.GetByIdAsync(request.CustomerId);

            Basket basket;

            if (basketResult.IsFailure)
            {
                basket = Basket.Create(request.CustomerId);
                basket.UpdateBasket(request.UpdateBasketItemRequest);
                return Result.Success(basket);
            }

            basket = basketResult.Value;

            return Result.Success(basket);
        }
    }
}
