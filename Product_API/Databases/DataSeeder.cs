using Bogus;
using Product_API.Domains.Categories;
using Product_API.Domains.Comments;
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
            "All",
            "Best Sellers",
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
        var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

        if (dbContext.Products.Count() >= 15)
            return;

        var faker = new Faker();
        List<Product> products = [];

        var commerceFaker = faker.Commerce;
        var categoryIdList = dbContext.Categories.Select(c => c.CategoryId).ToList();

        for (var i = 0; i < 15; i++)
        {
            List<ProductVariant> productVariants = [];

            List<string> imageUrls = [];

            for (var j = 0; j < 4; j++)
            {
                imageUrls.Add(faker.Image.PicsumUrl());
            }

            var product = Product.Create(
                commerceFaker.ProductName(),
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
                faker.Random.Number(1, 5),
                commerceFaker.ProductDescription()
            );
            var comment2 = Comment.Create(
                Guid.NewGuid(),
                faker.Random.Number(1, 5),
                commerceFaker.ProductDescription()
            );
            var comment3 = Comment.Create(
                Guid.NewGuid(),
                faker.Random.Number(1, 5),
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
            product.UpdatePrice();

            products.Add(product);
        }

        dbContext.AddRange(products);
        dbContext.SaveChanges();
    }
}
