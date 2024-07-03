using Basket_API.Domain.Baskets;
using Basket_API.Interfaces;
using FluentValidation;
using MediatR;
using Shared.Domain.Results;

namespace Basket_API.Features.Baskets;

public static class CreateBasket
{
    public record Command(string CustomerId) : IRequest<Result<Basket>>;

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
                    new("CreateBasket.Command", validationResult.Errors.ToString()!)
                );
            }

            var basket = Basket.Create(request.CustomerId);
            return await repository.CreateAsync(basket);
        }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.CustomerId).NotEmpty().WithMessage("CustomerId cannot be empty");
        }
    }
}
