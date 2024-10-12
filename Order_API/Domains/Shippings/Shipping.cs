using System.ComponentModel.DataAnnotations;
using Base.Bases;

namespace Order.API.Domains.ShippingMethods;

public class Shipping : AggregateRoot
{
    private Shipping() { }

    [MaxLength(255)]
    public Guid ShippingId
    {
        get => Id;
        private init => Id = value;
    }

    [MaxLength(100)]
    public string Name { get; private set; } = null!;

    public decimal Price { get; private set; }

    public static Shipping Create(string name)
    {
        return new Shipping { Name = name };
    }

    public void SetPrice(decimal price)
    {
        if (price < 0)
            throw new ArgumentException("Price can not be negative");

        Price = price;
    }

    public void Update(string name)
    {
        Name = name;
    }
}

public enum ShippingMethod
{
    Grab,
    Be,
    Ghtk,
}
