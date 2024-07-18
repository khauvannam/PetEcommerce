using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BaseDomain.Bases;
using Product_API.Domains.Products;

namespace Product_API.Domains.Categories;

public class Category : AggregateRoot
{
    private Category() { }

    [JsonInclude]
    [MaxLength(255)]
    public string CategoryId
    {
        get => Id;
        private set => Id = value;
    }

    [MaxLength(255)]
    public string CategoryName { get; private set; }

    [MaxLength(2000)]
    public string Description { get; private set; }

    [MaxLength(500)]
    public string ImageUrl { get; private set; }

    [JsonInclude]
    public List<Product> Products { get; set; }

    public static Category Create(string categoryName, string description, string imageUrl)
    {
        return new()
        {
            CategoryName = categoryName,
            Description = description,
            ImageUrl = imageUrl,
        };
    }

    public void Update(string categoryName, string description, string imageUrl)
    {
        CategoryName = categoryName;
        Description = description;
        ImageUrl = imageUrl;
    }
}
