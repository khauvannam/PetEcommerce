using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BaseDomain.Bases;

namespace Product_API.Domains.Discounts;

public class Discount : Entity
{
    private Discount() { }

    [MaxLength(255)]
    public Guid DiscountId
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

    [MaxLength(100)]
    public string Name { get; private set; } = null!;

    public decimal Percent { get; private set; }

    [MaxLength(255)]
    public Guid? CategoryId { get; private set; }

    [MaxLength(255)]
    public string ProductIdListJson { get; private set; } = null!;

    public DiscountStatus Status { get; private set; } = DiscountStatus.Happening;
    public DateTime StartDate { get; private set; } = DateTime.Now;
    public DateTime EndDate { get; private set; }

    public static Discount Create(
        string name,
        decimal percent,
        Guid? categoryId,
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
