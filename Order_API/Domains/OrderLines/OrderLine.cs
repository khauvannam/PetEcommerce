using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BasedDomain.Bases;
using Order.API.Domains.Orders;

namespace Order.API.Domains.OrderLines;

public class OrderLine : Entity
{
    private OrderLine() { }

    [MaxLength(255)]
    public Guid OrderLineId
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
    public Guid ProductId { get; private set; }

    [MaxLength(255)]
    public Guid OrderId { get; set; }

    [JsonIgnore]
    public OrderModel OrderModel { get; set; } = null!;

    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    public static OrderLine Create(Guid productId, int quantity, decimal price)
    {
        return new OrderLine
        {
            ProductId = productId,
            Quantity = quantity,
            Price = price,
        };
    }
}
