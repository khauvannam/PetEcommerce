using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BaseDomain.Bases;
using Order.API.Domains.Orders;

namespace Order.API.Domains.OrderLines;

public class OrderLine : Entity
{
    private OrderLine() { }

    [MaxLength(255)]
    public string OrderLineId
    {
        get => Id;
        private set => Id = value;
    }

    [MaxLength(255)]
    public string ProductId { get; private set; } = null!;

    [MaxLength(255)]
    public string OrderId { get; set; } = null!;

    [JsonIgnore]
    public OrderModel OrderModel { get; set; } = null!;

    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    public static OrderLine Create(string productId, int quantity, decimal price)
    {
        return new()
        {
            ProductId = productId,
            Quantity = quantity,
            Price = price
        };
    }
}
