using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BaseDomain.Bases;
using Product_API.Domains.Categories;

namespace Product_API.Domains.Products;

public class Product : AggregateRoot
{
    private Product() { }

    [JsonInclude]
    [MaxLength(255)]
    public string ProductId
    {
        get => Id;
        private set => Id = value;
    }

    [MaxLength(255)]
    public string Name { get; private set; } = null!;

    [MaxLength(2000)]
    public string Description { get; private set; } = null!;

    [MaxLength(2000)]
    public string ProductUseGuide { get; private set; } = null!;

    [MaxLength(500)]
    public string ImageUrl { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    [JsonInclude]
    [MaxLength(255)]
    public string CategoryId { get; set; } = null!;

    [Newtonsoft.Json.JsonIgnore]
    public Category Category { get; set; } = null!;

    [JsonInclude]
    public List<ProductVariant> ProductVariants { get; } = [];

    public static Product Create(
        string name,
        string description,
        string productUseGuide,
        string imageUrl,
        string categoryId
    )
    {
        return new()
        {
            Name = name,
            Description = description,
            CategoryId = categoryId,
            ProductUseGuide = productUseGuide,
            ImageUrl = imageUrl,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }

    public void UpdateProduct(
        string name,
        string description,
        string productUseGuide,
        string imageUrl,
        string categoryId,
        List<ProductVariant> productVariants
    )
    {
        Name = name;
        Description = description;
        ProductUseGuide = productUseGuide;
        ImageUrl = imageUrl;
        UpdatedAt = DateTime.Now;
        CategoryId = categoryId;
        UpdateVariantList(productVariants);
    }

    public void AddProductVariants(ProductVariant productVariant)
    {
        ProductVariants.Add(productVariant);
    }

    private void UpdateVariantList(List<ProductVariant> updateVariants)
    {
        ProductVariants.Clear();
        ProductVariants.AddRange(updateVariants);
    }

    /*private List<ProductVariant>? RemoveVariantIfNotInUpdateList(
        List<ProductVariant> updatedVariants
    )
    {
        if (ProductVariants.SequenceEqual(updatedVariants))
        {
            return null;
        }
        ProductVariants.RemoveAll(p => !updatedVariants.Contains(p));
        return ProductVariants;
    }

    private void SwapVariantPosition(List<ProductVariant> updatedVariants)
    {
        foreach (var productVariant in ProductVariants.ToList())
        {
            var newIndex = updatedVariants.FindIndex(v => v.VariantId == productVariant.VariantId);
            var oldIndex = ProductVariants.FindIndex(v =>
                v.VariantId == updatedVariants[newIndex].VariantId
            );
            if (newIndex == oldIndex)
                return;
            if (Math.Min(oldIndex, newIndex) < 0)
                return;
            (ProductVariants[oldIndex], ProductVariants[newIndex]) = (
                ProductVariants[newIndex],
                ProductVariants[oldIndex]
            );
        }
    }

    private void AddVariantIfNotExit(List<ProductVariant> updatedVariants)
    {
        ProductVariants.AddRange(updatedVariants);
    }
*/
}
