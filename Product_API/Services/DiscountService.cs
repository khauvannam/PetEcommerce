using System.Collections.Concurrent;
using Coravel.Queuing.Interfaces;
using Product_API.Databases;

namespace Product_API.Services;

public class DiscountService(ProductDbContext context, IQueue queue)
{
    private readonly ConcurrentDictionary<int, CancellationTokenSource> _cancellations = new();

    public async Task SetDiscountEnd(int discountId)
    {
        var discount = context.Discounts.FirstOrDefault(d => d.DiscountId == discountId)!;
        var timeUntilEnd = discount.EndDate - DateTime.Now;

        if (timeUntilEnd.TotalMilliseconds > 0)
        {
            var cts = new CancellationTokenSource();
            _cancellations[discountId] = cts;
            async Task WorkItem()
            {
                try
                {
                    await Task.Delay(timeUntilEnd, cts.Token);
                    if (!cts.Token.IsCancellationRequested)
                    {
                        discount.SetDiscountEnd();
                    }
                }
                catch (TaskCanceledException)
                {
                    throw new TaskCanceledException(
                        $"Discount end job for {discountId} was cancelled."
                    );
                }
                finally
                {
                    _cancellations.TryRemove(discountId, out _);
                }
            }

            queue.QueueAsyncTask(WorkItem);
        }

        await context.SaveChangesAsync();
    }

    public void CancelDiscountEnd(int discountId)
    {
        if (_cancellations.TryGetValue(discountId, out var cts))
        {
            cts.Cancel();
            _cancellations.TryRemove(discountId, out _);
        }
    }
}
