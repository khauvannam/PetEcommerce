using Hangfire;
using Product_API.Databases;

namespace Product_API.Services;

public class DiscountService(ProductDbContext context)
{
    public async Task SetDiscountEnd(string discountId)
    {
        var discount = context.Discounts.FirstOrDefault(d => d.DiscountId == discountId)!;
        BackgroundJob.Schedule(() => discount.SetDiscountEnd(), discount.EndDate);
        await context.SaveChangesAsync();
    }
}
