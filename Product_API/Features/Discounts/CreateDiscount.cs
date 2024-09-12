using BaseDomain.Results;
using MediatR;
using Newtonsoft.Json;
using Product_API.Domains.Discounts;
using Product_API.Errors;
using Product_API.Events.DiscountEvents;
using Product_API.Interfaces;
using Product_API.Services;

namespace Product_API.Features.Discounts;

public static class CreateDiscount
{
    public class Command : IRequest<Result>
    {
        public string Name { get; set; } = null!;
        public decimal Percent { get; set; } = 0;
        public List<string> ProductIdList { get; } = [];
        public string? CategoryId { get; } = null;
        public DateTime StartDate { get; } = DateTime.Now;
        public DateTime EndDate { get; } = default;
        public int Priority { get; } = 0;
    }

    public class Handler(
        IMediator mediator,
        IDiscountRepository discountRepository,
        DiscountService service
    ) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.CategoryId) && request.ProductIdList.Count <= 0)
                return Result.Failure(DiscountErrors.CreateForNothing());

            var productIdListJson = JsonConvert.SerializeObject(request.ProductIdList);

            var discount = Discount.Create(
                request.Name,
                request.Percent,
                request.CategoryId,
                productIdListJson,
                request.StartDate,
                request.EndDate
            );

            var result = await discountRepository.CreateAsync(discount);

            if (!result.IsFailure)
            {
                var @event = CreateDiscountEvent.Create(
                    request.Percent,
                    request.CategoryId,
                    request.ProductIdList,
                    request.Priority
                );
                await mediator.Publish(@event, cancellationToken);
                await service.SetDiscountEnd(discount.DiscountId);
            }

            return result;
        }
    }
}
