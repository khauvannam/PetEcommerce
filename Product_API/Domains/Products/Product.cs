using System.ComponentModel.DataAnnotations;
using BaseDomain.Bases;

namespace Product_API.Domains.Products;

public class Product : AggregateRoot
{
    private Product() { }

    [Key]
    public string ProductId
    {
        get => Id;
        private set => Id = value;
    }

    [MaxLength(255)]
    public string Name { get; private set; }

    public string Description { get; private set; }
    public string ProductUseGuide { get; private set; }
    public string ImageUrl { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public ProductCategory ProductCategory { get; private set; }
    public List<ProductVariant> ProductVariants { get; } = [];

    public static Product Create(
        string name,
        string description,
        string productUseGuide,
        string imageUrl,
        ProductCategory productCategory
    )
    {
        return new()
        {
            Name = name,
            Description = description,
            ProductCategory = productCategory,
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
        ProductCategory productCategory,
        List<ProductVariant> productVariants
    )
    {
        Name = name;
        Description = description;
        ProductUseGuide = productUseGuide;
        ImageUrl = imageUrl;
        UpdatedAt = DateTime.Now;
        UpdateCategory(productCategory);
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

    private void UpdateCategory(ProductCategory other)
    {
        if (ProductCategory.Equals(other))
        {
            return;
        }

        ProductCategory = other;
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
