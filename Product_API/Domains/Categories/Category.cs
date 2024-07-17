namespace Product_API.Domains.Categories;

public class Category
{
    private Category() { }

    public string CategoryId = Guid.NewGuid().ToString();
    public string CategoryName { get; private set; }
    public string Description { get; private set; }
    public string ImageUrl { get; private set; }

    public static Category Create(string categoryName, string description, string imageUrl) =>
        new()
        {
            CategoryName = categoryName,
            Description = description,
            ImageUrl = imageUrl,
        };

    public void Update(string categoryName)
    {
        CategoryName = categoryName;
    }
}
