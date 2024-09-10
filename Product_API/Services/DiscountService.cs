using Hangfire;
using MediatR;
using Product_API.Databases;
using Product_API.Events.DiscountEvents;

namespace Product_API.Services;

public class DiscountService(ProductDbContext context, IMediator mediator)
{
    public async Task SetDiscountEnd(string discountId)
    {
        var discount = context.Discounts.FirstOrDefault(d => d.DiscountId == discountId)!;
        BackgroundJob.Schedule(() => discount.SetDiscountEnd(), discount.EndDate);

        await context.SaveChangesAsync();
    }
}
