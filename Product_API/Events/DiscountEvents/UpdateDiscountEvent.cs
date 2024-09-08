using MediatR;

namespace Product_API.Events.DiscountEvents;

public record UpdateDiscountEvent(string DiscountId, decimal Percent) : INotification;
