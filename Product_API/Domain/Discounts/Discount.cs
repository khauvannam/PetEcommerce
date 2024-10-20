using System.ComponentModel.DataAnnotations;

namespace Product_API.Domain.Discounts;

public class Discount
{
    private Discount() { }

    [MaxLength(255)]
    public int DiscountId { get; private init; }

    [MaxLength(100)]
    public string Name { get; private set; } = null!;

    public decimal Percent { get; private set; }

    [MaxLength(255)]
    public int? CategoryId { get; private set; }

    [MaxLength(255)]
    public string ProductIdListJson { get; private set; } = null!;

    public DiscountStatus Status { get; private set; } = DiscountStatus.Happening;
    public DateTime StartDate { get; private set; } = DateTime.Now;
    public DateTime EndDate { get; private set; }

    public static Discount Create(
        string name,
        decimal percent,
        int? categoryId,
        string productIdListJson,
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
            CategoryId = categoryId,
            ProductIdListJson = productIdListJson,
        };
    }

    public void Update(string name, decimal percent, DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
            throw new ArgumentException("Start date cannot be greater than end date");

        Name = name;
        Percent = percent;
        StartDate = startDate;
        EndDate = endDate;
    }

    public void SetDiscountEnd()
    {
        Status = DiscountStatus.End;
    }
}

public enum DiscountStatus
{
    Happening,
    End,
}
