using Product_API.Domains.Categories;
using Product_API.Domains.Products;

namespace Product_API.Domains.Discounts;

public record DiscountRequest
{
    public decimal Value { get; private set; }
    public List<Product>? Products { get; private set; } = default;
    public string? CategoryId { get; private set; } = default;

    public static DiscountRequest Create(decimal value, List<Product>? products, string? categoryId)
    {
        if (products == null && categoryId == null)
        {
            throw new ArgumentException("You have to specific one of them the discount value");
        }

        return new()
        {
            Value = value,
            Products = products,
            CategoryId = categoryId
        };
    }
}
