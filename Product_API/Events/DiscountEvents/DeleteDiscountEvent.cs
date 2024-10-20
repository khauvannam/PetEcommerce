using MediatR;

namespace Product_API.Events.DiscountEvents;

public record DeleteDiscountEvent(int DiscountId) : INotification;
