using Base.Results;
using MediatR;
using Product_API.Events.DiscountEvents;
using Product_API.Interfaces;
using Product_API.Services;

namespace Product_API.Features.Discounts;

public static class DeleteDiscount
{
    public record Command(Guid DiscountId) : IRequest<Result>;

    public class Handler(
        IDiscountRepository discountRepository,
        IMediator mediator,
        DiscountService service
    ) : IRequestHandler<Command, Result>
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

                service.CancelDiscountEnd(discount.DiscountId);
            }

            return result;
        }
    }
}
