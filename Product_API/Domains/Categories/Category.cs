namespace Product_API.Domains.Categories;

public class Category
{
    private Category() { }

    public string CategoryId = Guid.NewGuid().ToString();
    public string CategoryName { get; private set; }
    public string Description { get; private set; }
    public string ImageUrl { get; private set; }
    public HashSet<string> Details { get; private set; }

    public static Category Create(
        string categoryName,
        string description,
        string imageUrl,
        HashSet<string> details
    ) =>
        new()
        {
            CategoryName = categoryName,
            Description = description,
            ImageUrl = imageUrl,
            Details = details
        };

    public void Update(HashSet<string> details, string categoryName) =>
        (Details, CategoryName) = (details, categoryName);
}
