using MediatR;

namespace Product_API.Events.DiscountEvents;

public record DeleteDiscountEvent(string DiscountId) : INotification;
