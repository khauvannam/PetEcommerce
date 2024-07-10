namespace Product_API.Domain.Categories;

public class Category
{
    private Category(string categoryName, HashSet<string> details)
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
    public HashSet<string> Details { get; private set; }

    public static Category Create(string categoryName, HashSet<string> details) =>
        new(categoryName, details);

    public void Update(HashSet<string> details, string categoryName) =>
        (Details, CategoryName) = (details, categoryName);
}
