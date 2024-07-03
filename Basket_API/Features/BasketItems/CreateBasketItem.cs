using Basket_API.Domain.BasketItems;
using Basket_API.Interfaces;
using FluentValidation;
using MediatR;
using Shared.Domain.Results;

namespace Basket_API.Features.BasketItems
{
    public static class CreateBasketItem
    {
        public record Command(
            string ProductId,
            string VariantId,
            string Name,
            int Quantity,
            decimal Price,
            bool OnSale,
            string ImageUrl
        ) : IRequest<Result<BasketItem>>;

        internal sealed class Handler(
            IBasketItemRepository repository,
            IValidator<Command> validator
        ) : IRequestHandler<Command, Result<BasketItem>>
        {
            public async Task<Result<BasketItem>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return Result.Failure<BasketItem>(
                        new("CreateBasketItem.Command", validationResult.Errors.ToString()!)
                    );
                }
                var basketItem = BasketItem.Create(
                    request.ProductId,
                    request.VariantId,
                    request.Name,
                    Quantity.Create(request.Quantity),
                    request.Price,
                    request.OnSale,
                    request.ImageUrl
                );
                return await repository.CreateAsync(basketItem);
            }
        }

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.ProductId).NotEmpty().WithMessage("ProductId cannot be empty");
                RuleFor(c => c.VariantId).NotEmpty().WithMessage("VariantId cannot be empty");
                RuleFor(c => c.Name).NotEmpty().WithMessage("Name cannot be empty");
                RuleFor(c => c.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Quantity must be greater than zero");
                RuleFor(c => c.Price).GreaterThan(0).WithMessage("Price must be greater than zero");
                RuleFor(c => c.ImageUrl).NotEmpty().WithMessage("ImageUrl cannot be empty");
            }
        }
    }
}
