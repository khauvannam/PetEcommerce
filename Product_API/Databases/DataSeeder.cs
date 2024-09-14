﻿using Bogus;
using Microsoft.EntityFrameworkCore;
using Product_API.Domains.Categories;
using Product_API.Domains.Products;

namespace Product_API.Databases;

public static class DataSeeder
{
    public static void SeedCategory(IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

        if (dbContext.Categories.Count() >= 10)
        {
            return;
        }

        var faker = new Faker();
        List<Category> categories = [];
        List<string> categoryNames =
        [
            "Dog Food",
            "Cat Food",
            "Pet Toys",
            "Aquarium Supplies",
            "Bird Accessories",
            "Reptile Supplies",
            "Grooming Tools",
            "Pet Health",
            "Pet Clothing",
            "Pet Beds",
        ];
        for (var i = 0; i < 10; i++)
        {
            var category = Category.Create(
                faker.PickRandom(categoryNames),
                faker.Commerce.ProductDescription(),
                faker.Image.PicsumUrl()
            );
            categories.Add(category);
        }
        dbContext.AddRange(categories);
        dbContext.SaveChanges();
    }

    public static void SeedProduct(IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

        if (dbContext.Products.Count() >= 15)
            return;

        var faker = new Faker();
        List<Product> products = [];
        var commerceFaker = faker.Commerce;
        var categoryIdList = dbContext.Categories.Select(c => c.CategoryId).ToList();

        for (var i = 0; i < 15; i++)
        {
            var product = Product.Create(
                commerceFaker.ProductName(),
                commerceFaker.ProductDescription(),
                commerceFaker.ProductMaterial(),
                faker.Image.PicsumUrl(),
                faker.PickRandom(categoryIdList)
            );

            var variant1 = ProductVariant.Create(
                commerceFaker.ProductName(),
                faker.Random.Number(1, 100)
            );
            variant1.SetPrice(faker.Random.Decimal(100, 10000));

            var variant2 = ProductVariant.Create(
                commerceFaker.ProductName(),
                faker.Random.Number(1, 100)
            );
            variant2.SetPrice(faker.Random.Decimal(100, 10000));

            product.AddProductVariants(variant1);
            product.AddProductVariants(variant2);

            product.ApplyDiscount(faker.Random.Decimal(10, 60));

            products.Add(product);
        }
        dbContext.AddRange(products);
        dbContext.SaveChanges();
    }
}