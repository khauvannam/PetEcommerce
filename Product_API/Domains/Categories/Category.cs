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
    public Guid CategoryId
    {
        get => Id;
        private init => Id = value;
    }

    [JsonIgnore]
    public new int ClusterId
    {
        get => base.ClusterId;
        init => base.ClusterId = value;
    }

    [MaxLength(255)]
    public string CategoryName { get; private set; } = null!;

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
        };
    }

    public void Update(string categoryName, string description, string imageUrl)
    {
        CategoryName = categoryName;
        Description = description;
        ImageUrl = imageUrl;
    }
}
