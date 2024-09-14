﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BaseDomain.Bases;

namespace Product_API.Domains.Products;

public class Product : AggregateRoot
{
    private Product() { }

    [JsonInclude]
    [MaxLength(255)]
    public Guid ProductId
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
    public string Name { get; private set; } = null!;

    [MaxLength(2000)]
    public string Description { get; private set; } = null!;

    [MaxLength(2000)]
    public string ProductUseGuide { get; private set; } = null!;

    [MaxLength(500)]
    public string ImageUrl { get; private set; } = null!;

    public DiscountPercent DiscountPercent { get; set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    [JsonInclude]
    [MaxLength(255)]
    public Guid CategoryId { get; set; }

    [JsonInclude]
    public List<ProductVariant> ProductVariants { get; } = [];

    public static Product Create(
        string name,
        string description,
        string productUseGuide,
        string imageUrl,
        Guid categoryId
    )
    {
        return new Product
        {
            Name = name,
            Description = description,
            CategoryId = categoryId,
            ProductUseGuide = productUseGuide,
            ImageUrl = imageUrl,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
        };
    }

    public void UpdateProduct(
        string name,
        string description,
        string productUseGuide,
        string imageUrl,
        Guid categoryId,
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
        if (ProductVariants.Count > 5)
            throw new InvalidOperationException(
                "Product variant list cannot contain more than 5 parts"
            );

        ProductVariants.Add(productVariant);
    }

    public void ApplyDiscount(decimal percent)
    {
        DiscountPercent = DiscountPercent.Create(percent);
    }

    private void UpdateVariantList(List<ProductVariant> updateVariants)
    {
        ProductVariants.Clear();
        ProductVariants.AddRange(updateVariants);
    }
}

public class DiscountPercent : ValueObject
{
    private DiscountPercent() { }

    public decimal Value { get; private init; }

    public static DiscountPercent Create(decimal value)
    {
        if (value is < 0 or > 100)
            throw new ArgumentException("DiscountPercent value must be between 0 and 100");

        return new DiscountPercent { Value = value };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
