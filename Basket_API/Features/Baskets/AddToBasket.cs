using BaseDomain.Results;
using Basket_API.Domains.BasketItems;
using Basket_API.Domains.Baskets;
using Basket_API.Interfaces;
using FluentValidation;
using MediatR;

namespace Basket_API.Features.Baskets;

public static class AddToBasket
{
    public record Command(
        Guid BasketId,
        Guid CustomerId,
        List<BasketItemRequest> BasketItemRequests
    ) : IRequest<Result<Basket>>;

    internal sealed class Handler(IBasketRepository repository, IValidator<Command> validator)
        : IRequestHandler<Command, Result<Basket>>
    {
        public async Task<Result<Basket>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return Result.Failure<Basket>(
                    new ErrorType("UpdateBasket.Command", validationResult.Errors.ToString()!)
                );

            Basket basket;
            var result = await repository.GetByIdAsync(request.BasketId);
            if (result.IsFailure)
            {
                basket = Basket.Create(request.CustomerId);

                foreach (var basketItemRequest in request.BasketItemRequests)
                {
                    var basketItem = BasketItem.Create(
                        basketItemRequest.ProductId,
                        basketItemRequest.VariantId,
                        basketItemRequest.Name,
                        Quantity.Create(basketItemRequest.Quantity),
                        basketItemRequest.Price,
                        basketItemRequest.ImageUrl
                    );

                    basket.BasketItemsList.Add(basketItem);
                }

                return await repository.CreateAsync(basket);
            }

            basket = result.Value;
            if (request.BasketItemRequests.Count == 0)
                basket.RemoveAllBasketItem();

            basket.RemoveAllBasketItemNotExist(request.BasketItemRequests);
            foreach (var basketItemRequest in request.BasketItemRequests)
                // CHANGE QUANTITY IF EXIST OR CREATE NEW BASKET ITEM IF NOT
                basket.UpdateBasket(basketItemRequest);

            return await repository.UpdateAsync(basket);
        }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.BasketId).NotEmpty().WithMessage("BasketId cannot be empty");
        }
    }
}
