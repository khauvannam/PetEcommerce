namespace Product_API.Domain.Categories;

public class Category
{
    private Category(string categoryName, List<string> details)
    {
        if (details.Count > 15)
        {
            throw new ArgumentException("Details is limited at 15");
        }

        CategoryName = categoryName;
        Details = details;
    }

    public string CategoryId = Guid.NewGuid().ToString();
    public string CategoryName { get; private set; }
    public List<string> Details { get; private set; }

    public static Category Create(string categoryName, List<string> details) =>
        new(categoryName, details);

    public void Update(List<string> details, string categoryName) =>
        (Details, CategoryName) = (details, categoryName);
}
