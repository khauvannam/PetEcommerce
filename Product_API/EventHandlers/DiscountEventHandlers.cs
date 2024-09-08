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
            return Task.CompletedTask;
        }
    }
}
