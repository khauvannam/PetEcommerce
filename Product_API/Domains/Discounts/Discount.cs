using System.ComponentModel.DataAnnotations;
using BaseDomain.Bases;

namespace Product_API.Domains.Discounts;

public class Discount : Entity
{
    private Discount() { }

    [MaxLength(100)]
    public string Name { get; private set; } = null!;

    public decimal Percent { get; private set; }
    public DateTime StartDate { get; private set; } = DateTime.Now;

    public DateTime EndDate { get; private set; }

    public static Discount Create(
        string name,
        decimal percent,
        DateTime startDate,
        DateTime endDate
    )
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date cannot be greater than end date");

        return new Discount
        {
            Name = name,
            Percent = percent,
            StartDate = startDate,
            EndDate = endDate,
        };
    }
}
