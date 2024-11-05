using Base.Results;
using Basket_API.Domain.BasketItems;
using Basket_API.Domain.Baskets;
using Basket_API.DTOs.BasketItems;
using Basket_API.Interfaces;
using FluentValidation;
using MediatR;

namespace Basket_API.Features.Baskets;

public static class UpdateBasket
{
    public record Command(int CustomerId, List<UpdateBasketItemRequest> BasketItemRequests)
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
                var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                return Result.Failure<Basket>(new(nameof(Command), $"Invalid request : {errors}"));
            }

            var result = await repository.GetByIdAsync(request.CustomerId);
            var basket = result.Value!;

            if (request.BasketItemRequests.Count == 0)
            {
                basket.RemoveAllBasketItem();
                return await repository.UpdateAsync(basket);
            }

            basket.RemoveAllBasketItemNotExist(request.BasketItemRequests);

            foreach (var basketItemRequest in request.BasketItemRequests)
            // CHANGE QUANTITY IF EXIST OR CREATE NEW BASKET ITEM IF NOT
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
            RuleFor(c => c.CustomerId)
                .NotEmpty()
                .WithMessage("You must provide a valid customer id");
        }
    }
}
