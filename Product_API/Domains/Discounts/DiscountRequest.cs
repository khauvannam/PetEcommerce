using Product_API.Domains.Products;

namespace Product_API.Domains.Discounts;

public record DiscountRequest
{
    public decimal Value { get; } = 0;
    public List<Product> Products { get; } = [];
    public string? CategoryId { get; } = null;
    public DateTime StartDate { get; private set; } = DateTime.Now;
    public DateTime EndDate { get; private set; } = default;
}
