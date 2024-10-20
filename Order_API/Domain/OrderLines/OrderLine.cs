using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Base.Bases;
using Order.API.Domain.Orders;

namespace Order.API.Domain.OrderLines;

public class OrderLine : Entity
{
    private OrderLine() { }

    [MaxLength(255)]
    public int OrderLineId
    {
        get => Id;
        private init => Id = value;
    }

    [MaxLength(255)]
    public int ProductId { get; private set; }

    [MaxLength(255)]
    public int OrderId { get; set; }

    [JsonIgnore]
    public OrderModel OrderModel { get; set; } = null!;

    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    public static OrderLine Create(int productId, int quantity, decimal price)
    {
        return new OrderLine
        {
            ProductId = productId,
            Quantity = quantity,
            Price = price,
        };
    }
}
