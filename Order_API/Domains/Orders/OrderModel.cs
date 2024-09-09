using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BaseDomain.Bases;
using Order.API.Domains.OrderLines;

namespace Order.API.Domains.Orders;

public class OrderModel : AggregateRoot
{
    private OrderModel() { }

    [MaxLength(255)]
    public string OrderId
    {
        get => Id;
        private set => Id = value;
    }

    [MaxLength(255)]
    public string UserId { get; private set; } = null!;

    public DateTime OrderDate { get; private init; }
    public decimal OrderTotal { get; private set; }
    public OrderStatus OrderStatus { get; private set; }

    [MaxLength(255)]
    public string ShippingMethodId { get; set; } = null!;

    [MaxLength(500)]
    public string ShippingAddress { get; private set; } = null!;

    [JsonInclude]
    public List<OrderLine> OrderLines { get; set; } = null!;

    public static OrderModel Create(
        string userId,
        string shippingAddress,
        OrderStatus orderStatus = OrderStatus.Pending
    )
    {
        return new OrderModel
        {
            UserId = userId,
            OrderStatus = orderStatus,
            ShippingAddress = shippingAddress,
            OrderDate = DateTime.Now,
            OrderLines = [],
        };
    }

    public void AddOrderLine(OrderLine orderLine)
    {
        OrderLines.Add(orderLine);
        OrderTotal += orderLine.Price;
    }

    public void UpdateOrderStatus(OrderStatus orderStatus)
    {
        OrderStatus = orderStatus;
    }
}

public enum OrderStatus
{
    Pending = 0,
    Shipping = 1,
    Finish = 2,
}
