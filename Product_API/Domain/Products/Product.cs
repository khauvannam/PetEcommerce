using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Domain.Bases;

namespace Product_API.Domain.Products;

public class Product : AggregateRoot
{
    private Product(string name, string description, ProductCategory productCategory)
    {
        Name = name;
        Description = description;
        ProductCategory = productCategory;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    [BsonId]
    public string ProductId => Id;
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public ProductCategory ProductCategory { get; private set; }
    private List<ProductVariant> ProductVariants { get; } = [];

    public static Product Create(string name, string description, ProductCategory productCategory)
    {
        return new(name, description, productCategory);
    }

    public void UpdateProduct(
        string name,
        ProductCategory productCategory,
        VariantUpdatedStatus status,
        List<ProductVariant> productVariants
    )
    {
        Name = name;
        UpdatedAt = DateTime.Now;
        UpdateCategory(productCategory);
        UpdateVariantList(productVariants, status);
    }

    public void AddProductVariants(ProductVariant productVariant)
    {
        ProductVariants.Add(productVariant);
    }

    private void UpdateVariantList(
        List<ProductVariant> updateVariants,
        VariantUpdatedStatus status = VariantUpdatedStatus.None
    )
    {
        switch (status)
        {
            case VariantUpdatedStatus.None:
                break;
            case VariantUpdatedStatus.ReplaceAll:
                ProductVariants.Clear();
                ProductVariants.AddRange(updateVariants);
                break;
            case VariantUpdatedStatus.ChangePosition:
                SwapVariantPosition(updateVariants);
                break;
            case VariantUpdatedStatus.AddMoreVariant:
                AddVariantIfNotExit(updateVariants);
                break;
            case VariantUpdatedStatus.JustRemove:
                RemoveVariantIfNotInUpdateList(updateVariants);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(status));
        }
    }

    private List<ProductVariant>? RemoveVariantIfNotInUpdateList(
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

    private void UpdateCategory(ProductCategory other)
    {
        if (ProductCategory.Equals(other))
        {
            return;
        }

        ProductCategory = other;
    }
}

public class ProductCategory : ValueObject
{
    public string ProductCategoryId { get; } = null!;
    public BsonDocument Details { get; set; } = null!;

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return ProductCategoryId;
        yield return Details;
    }
}

public enum VariantUpdatedStatus
{
    None = 0,
    ReplaceAll = 1,
    ChangePosition = 2,
    AddMoreVariant = 3,
    JustRemove = 4
}
