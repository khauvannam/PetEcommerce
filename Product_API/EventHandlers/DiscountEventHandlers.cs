using MediatR;
using Product_API.Databases;
using Product_API.Events.DiscountEvents;

namespace Product_API.EventHandlers;

public static class DiscountEventHandlers
{
    public class CreateDiscountEventHandler(ProductDbContext context)
        : INotificationHandler<CreateDiscountEvent>
    {
        public Task Handle(CreateDiscountEvent notification, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(notification.CategoryId))
                foreach (
                    var product in context.Products.Where(p =>
                        p.CategoryId == notification.CategoryId
                    )
                )
                    product.ApplyDiscount(notification.Percent);

            if (notification.ProductIdList.Count > 0)
                foreach (
                    var product in context.Products.Where(p =>
                        notification.ProductIdList.Contains(p.ProductId)
                    )
                )
                    product.ApplyDiscount(notification.Percent);
            context.SaveChanges();

            return Task.CompletedTask;
        }
    }
}
