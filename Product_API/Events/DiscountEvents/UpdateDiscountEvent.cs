using MediatR;
using Product_API.Domains.Discounts;

namespace Product_API.Events.DiscountEvents;

public record UpdateDiscountEvent(Discount Discount, decimal Percent) : INotification;
