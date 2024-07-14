namespace Client_App.Domains.Categories;

public class Category
{
    public string CategoryId { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public HashSet<string> Details { get; set; } = null!;
}
