using System.ComponentModel.DataAnnotations;
using BaseDomain.Bases;

namespace Order.API.Domains.ShippingMethods;

public class ShippingMethod : AggregateRoot
{
    private ShippingMethod() { }

    [MaxLength(255)]
    public string ShippingMethodId
    {
        get => Id;
        private set => Id = value;
    }

    [MaxLength(100)]
    public string Name { get; private set; } = null!;

    public decimal Price { get; private set; }

    public static ShippingMethod Create(string name)
    {
        return new ShippingMethod { Name = name };
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
