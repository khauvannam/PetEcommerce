using System.ComponentModel.DataAnnotations;
using BaseDomain.Bases;

namespace Product_API.Domains.Discounts;

public class Discount : Entity
{
    private Discount() { }

    [MaxLength(100)]
    public string Name { get; private set; } = null!;

    public decimal Percent { get; private set; }

    public static Discount Create(string name, decimal percent)
    {
        return new() { Name = name, Percent = percent };
    }
}
