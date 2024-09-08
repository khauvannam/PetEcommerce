using Product_API.Domains.Products;

namespace Product_API.Domains.Discounts;

public record DiscountRequest
{
    public string Name { get; init; } = string.Empty;
    public decimal Percent { get; } = 0;
    public DateTime StartDate { get; private set; } = DateTime.Now;
    public DateTime EndDate { get; private set; } = default;
}
