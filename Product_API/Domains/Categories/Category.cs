using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BasedDomain.Bases;
using Product_API.Domains.Products;

namespace Product_API.Domains.Categories;

public class Category
{
    private Category() { }

    public int CategoryId { get; init; }

    [MaxLength(255)]
    public string CategoryName { get; private set; } = null!;

    [MaxLength(255)]
    public string Endpoint { get; private set; } = null!;

    [MaxLength(2000)]
    public string Description { get; private set; } = null!;

    [MaxLength(500)]
    public string ImageUrl { get; private set; } = null!;

    public static Category Create(string categoryName, string description, string imageUrl)
    {
        return new Category
        {
            CategoryName = categoryName,
            Description = description,
            ImageUrl = imageUrl,
            Endpoint = CreateEndpoint(categoryName),
        };
    }

    public void Update(string categoryName, string description, string imageUrl)
    {
        CategoryName = categoryName;
        Description = description;
        ImageUrl = imageUrl;
        Endpoint = CreateEndpoint(categoryName);
    }

    private static string CreateEndpoint(string categoryName)
    {
        var array = categoryName.ToLower().Split(" ");
        return string.Join('-', array);
    }
}
