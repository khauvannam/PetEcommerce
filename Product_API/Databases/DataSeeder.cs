﻿using Bogus;
using Product_API.Domains.Categories;
using Product_API.Domains.Comments;
using Product_API.Domains.Products;

namespace Product_API.Databases;

public class DataSeeder
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
            "All",
            "Best Sellers",
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
        categories.AddRange(
            categoryNames.Select(t =>
                Category.Create(t, faker.Commerce.ProductDescription(), faker.Image.PicsumUrl())
            )
        );

        dbContext.AddRange(categories);
        dbContext.SaveChanges();
    }

    public static void SeedProduct(IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        var random = new Random();
        var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

        if (dbContext.Products.Count() >= 100)
            return;

        var faker = new Faker();
        List<Product> products = [];

        var commerceFaker = faker.Commerce;

        List<string> excludedCategories = ["best-sellers", "all"];
        var categoryIdList = dbContext
            .Categories.Where(c => !excludedCategories.Contains(c.Endpoint))
            .Select(c => c.CategoryId)
            .ToList();

        for (var i = 0; i < 100; i++)
        {
            List<ProductVariant> productVariants = [];

            List<string> imageUrls = [];

            for (var j = 0; j < 4; j++)
            {
                imageUrls.Add(faker.Image.PicsumUrl());
            }

            var product = Product.Create(
                $"{commerceFaker.ProductName()}-{random.Next(1, 101)}",
                commerceFaker.ProductDescription(),
                commerceFaker.ProductDescription(),
                faker.PickRandom(categoryIdList)
            );

            for (var j = 0; j < 3; j++)
            {
                var variant = ProductVariant.Create(
                    commerceFaker.ProductName(),
                    faker.Random.Number(120, 1200)
                );
                variant.SetPrice(faker.Random.Decimal(100, 10000));
                productVariants.Add(variant);
            }

            var comment1 = Comment.Create(
                Guid.NewGuid(),
                faker.Person.Email,
                faker.Random.Number(1, 5),
                commerceFaker.ProductDescription(),
                commerceFaker.ProductDescription()
            );
            var comment2 = Comment.Create(
                Guid.NewGuid(),
                faker.Person.Email,
                faker.Random.Number(1, 5),
                commerceFaker.ProductDescription(),
                commerceFaker.ProductDescription()
            );
            var comment3 = Comment.Create(
                Guid.NewGuid(),
                faker.Person.Email,
                faker.Random.Number(1, 5),
                commerceFaker.ProductDescription(),
                commerceFaker.ProductDescription()
            );

            product.AddComment(comment1);
            product.AddComment(comment2);
            product.AddComment(comment3);

            product.AddProductVariants(productVariants);

            product.ApplyDiscount(faker.Random.Decimal(10, 60));
            product.UpdateSoldQuantity(faker.Random.Number(50, 150));
            product.CalculateTotalQuantity();
            product.CalculateTotalRating();
            product.AddImageUrl(imageUrls);

            products.Add(product);
        }

        dbContext.AddRange(products);
        dbContext.SaveChanges();
    }
}
