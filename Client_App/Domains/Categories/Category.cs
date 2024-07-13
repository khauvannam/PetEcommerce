namespace Client_App.Domains.Categories;

public class Category
{
    public string CategoryId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<string> Details { get; set; } = null!;
}
