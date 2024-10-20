using MediatR;
using Product_API.Domain.Discounts;

namespace Product_API.Events.DiscountEvents;

public record UpdateDiscountEvent(Discount Discount, decimal Percent) : INotification;
