using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Product_API.Databases;
using Product_API.Events.DiscountEvents;

namespace Product_API.EventHandlers;

public static class DiscountEventHandlers
{
    private static async Task ApplyDiscountToProductsAsync(
        ProductDbContext context,
        int? categoryId,
        List<int>? productIdList,
        decimal discountPercent
    )
    {
        // Apply discount to all products within the given category
        if (categoryId is not null)
        {
            var categoryProducts = await context
                .Products.Where(p => p.CategoryId == categoryId)
                .ToListAsync();

            foreach (var product in categoryProducts.Where(p => p.DiscountPercent.Value != 0))
            {
                product.ApplyDiscount(discountPercent);
            }
        }

        if (productIdList is { Count: > 0 })
        {
            var selectedProducts = await context
                .Products.Where(p => productIdList.Contains(p.ProductId))
                .ToListAsync();

            foreach (
                var product in selectedProducts.Where(product => product.DiscountPercent.Value == 0)
            )
            {
                product.ApplyDiscount(discountPercent);
            }
        }

        await context.SaveChangesAsync();
    }

    public class CreateDiscountEventHandler(ProductDbContext context)
        : INotificationHandler<CreateDiscountEvent>
    {
        public async Task Handle(
            CreateDiscountEvent notification,
            CancellationToken cancellationToken
        )
        {
            await ApplyDiscountToProductsAsync(
                context,
                notification.CategoryId,
                notification.ProductIdList,
                notification.Percent
            );
        }
    }

    public class DeleteDiscountEventHandler(ProductDbContext context)
        : INotificationHandler<DeleteDiscountEvent>
    {
        public async Task Handle(
            DeleteDiscountEvent notification,
            CancellationToken cancellationToken
        )
        {
            var discount = await context.Discounts.FirstOrDefaultAsync(
                d => d.DiscountId == notification.DiscountId,
                cancellationToken
            );

            if (discount is null)
                return;

            var productIdList =
                JsonConvert.DeserializeObject<List<int>>(discount.ProductIdListJson) ?? [];

            await ApplyDiscountToProductsAsync(
                context,
                discount.CategoryId,
                productIdList,
                default // Reset discount to default (0%)
            );
        }
    }

    public class UpdateDiscountEventHandler(ProductDbContext context)
        : INotificationHandler<UpdateDiscountEvent>
    {
        public async Task Handle(
            UpdateDiscountEvent notification,
            CancellationToken cancellationToken
        )
        {
            var discount = notification.Discount;

            var productIdList =
                JsonConvert.DeserializeObject<List<int>>(discount.ProductIdListJson) ?? [];

            await ApplyDiscountToProductsAsync(
                context,
                discount.CategoryId,
                productIdList,
                discount.Percent
            );
        }
    }
}
