using BaseDomain.Results;
using Basket_API.Domains.BasketItems;
using Basket_API.Domains.Baskets;
using Basket_API.Errors;
using Basket_API.Interfaces;
using FluentValidation;
using MediatR;

namespace Basket_API.Features.Baskets
{
    public static class UpdateBasket
    {
        public record Command(string BasketId, List<BasketItemRequest> BasketItemRequests)
            : IRequest<Result<Basket>>;

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
                {
                    return Result.Failure<Basket>(
                        new("UpdateBasket.Command", validationResult.Errors.ToString()!)
                    );
                }

                var result = await repository.GetByIdAsync(request.BasketId);
                if (result.IsFailure)
                {
                    return Result.Failure<Basket>(BasketErrors.NotFound);
                }

                var basket = result.Value;
                if (request.BasketItemRequests.Count == 0)
                {
                    basket.RemoveAllBasketItem();
                }
                basket.RemoveAllBasketItemNotExist(request.BasketItemRequests);
                foreach (var basketItemRequest in request.BasketItemRequests)
                {
                    basket.UpdateBasket(basketItemRequest);
                }
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
}
