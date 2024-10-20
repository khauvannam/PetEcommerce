namespace Client_App.DTOs.Categories.Responses;

public class Category
{
    public int CategoryId { get; set; }
    public string Endpoint { get; set; } = null!;
    public string? CategoryName { get; set; } = null!;

    public string? Description { get; set; } = null!;
    public string? ImageUrl { get; set; } = null!;
}
