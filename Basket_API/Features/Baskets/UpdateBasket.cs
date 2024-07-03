using Basket_API.Domain.Baskets;
using Basket_API.Errors;
using Basket_API.Interfaces;
using FluentValidation;
using MediatR;
using Shared.Domain.Results;

namespace Basket_API.Features.Baskets
{
    public static class UpdateBasket
    {
        public record Command(string BasketId, string CustomerId) : IRequest<Result<Basket>>;

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

                return await repository.UpdateAsync(basket);
            }
        }

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.BasketId).NotEmpty().WithMessage("BasketId cannot be empty");
                RuleFor(c => c.CustomerId).NotEmpty().WithMessage("CustomerId cannot be empty");
            }
        }
    }
}
