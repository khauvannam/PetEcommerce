using BasedDomain.Results;
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
        public List<Guid> ProductIdList { get; } = [];
        public int? CategoryId { get; } = null;
        public DateTime StartDate { get; } = DateTime.Now;
        public DateTime EndDate { get; } = default;
        public int Priority { get; } = 0;
    }

    public class Handler(
        IMediator mediator,
        IDiscountRepository repository,
        DiscountService service
    ) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var discounts = (await repository.GetAllAsync()).Value;
            if (discounts.Any(d => d.Status == DiscountStatus.Happening))
            {
                return Result.Failure(DiscountErrors.CurrentlyActive());
            }

            if (request.CategoryId is null && request.ProductIdList.Count <= 0)
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

            var result = await repository.CreateAsync(discount);

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
