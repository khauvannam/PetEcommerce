namespace Product_API.Domain.Categories;

public class Category(List<string> details)
{
    public string CategoryId = Guid.NewGuid().ToString();
    public List<string> Details { get; } = details;
}
