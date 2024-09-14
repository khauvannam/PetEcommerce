using MediatR;

namespace Product_API.Events.DiscountEvents;

public record DeleteDiscountEvent(Guid DiscountId) : INotification;
