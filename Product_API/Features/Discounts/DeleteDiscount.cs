using BaseDomain.Results;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product_API.Events.DiscountEvents;
using Product_API.Interfaces;

namespace Product_API.Features.Discounts;

public static class DeleteDiscount
{
    public record Command(string DiscountId) : IRequest<Result>;

    public class Handler(IDiscountRepository discountRepository, IMediator mediator)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await discountRepository.GetByIdAsync(request.DiscountId);
            if (!result.IsFailure)
            {
                var discount = result.Value;
                await discountRepository.DeleteAsync(discount);

                var @event = new DeleteDiscountEvent(discount.DiscountId);
                await mediator.Publish(@event, cancellationToken);
            }

            return result;
        }
    }
}
