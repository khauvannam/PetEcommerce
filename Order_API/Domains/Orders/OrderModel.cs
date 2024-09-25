using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BasedDomain.Bases;
using Order.API.Domains.OrderLines;
using Order.API.Domains.Payments;
using Order.API.Domains.ShippingMethods;

namespace Order.API.Domains.Orders;

public class OrderModel : AggregateRoot
{
    private OrderModel() { }

    [MaxLength(255)]
    public Guid OrderId
    {
        get => Id;
        private init => Id = value;
    }

    [JsonIgnore]
    public new int ClusterId
    {
        get => base.ClusterId;
        init => base.ClusterId = value;
    }

    [MaxLength(255)]
    public Guid UserId { get; private set; }

    public DateTime OrderDate { get; private init; }
    public decimal OrderTotal { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }

    public Payment Payment { get; private set; } = null!;

    public Shipping Shipping { get; set; } = null!;

    [MaxLength(500)]
    public string ShippingAddress { get; private set; } = null!;

    [JsonInclude]
    public List<OrderLine> OrderLines { get; set; } = null!;

    public static OrderModel Create(
        Guid userId,
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
            PaymentStatus =
                PaymentStatus.Pending // Cash case
            ,
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

    public void UpdatePaymentStatus(PaymentStatus paymentStatus)
    {
        PaymentStatus = paymentStatus;
    }

    public void ProcessOrder(Payment payment, Shipping shipping)
    {
        Payment = payment;
        Shipping = shipping;
    }
}

public enum OrderStatus
{
    Pending = 0,
    Finish = 2,
}

public enum PaymentStatus
{
    Pending,
    Completed,
    Failed,
}
