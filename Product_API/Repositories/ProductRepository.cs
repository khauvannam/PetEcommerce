using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Product_API.Databases;
using Product_API.Domain.Products;
using Product_API.Errors;
using Product_API.Features.Products;
using Product_API.Interfaces;
using Shared.Domain.Results;

namespace Product_API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IMongoCollection<Product> _productCollection;

    public ProductRepository(IOptions<ProductDatabaseSetting> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var database = client.GetDatabase(options.Value.DatabaseName);
        _productCollection = database.GetCollection<Product>(options.Value.CollectionName);
    }

    public async Task<Result<List<Product>>> ListAllProducts(ListAllProducts.Command command)
    {
        var filter = command.CategoryId is null
            ? Builders<Product>.Filter.Empty
            : Builders<Product>.Filter.Eq(
                p => p.ProductCategory.ProductCategoryId,
                command.CategoryId
            );
        var products = await _productCollection
            .Find(filter)
            .Skip(command.Offset)
            .Limit(command.Limit * command.Offset)
            .ToListAsync();
        return Result.Success(products);
    }

    public async Task<Result<Product>> CreateProduct(CreateProduct.Command command)
    {
        var productCategory = command.ProductCategory;
        var product = Product.Create(command.Name, command.Description, productCategory);
        foreach (var variant in command.ProductVariants)
        {
            var productVariant = ProductVariant.Create(variant.VariantName, variant.ImageUrl);
            productVariant.SetPrice(variant.OriginalPrice);
            productVariant.ApplyDiscount(variant.DiscountPercent);
            product.AddProductVariants(productVariant);
        }

        await _productCollection.InsertOneAsync(product);
        return Result.Success(product);
    }

    public async Task<Result> DeleteProduct(string productId, CancellationToken cancellationToken)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.ProductId, productId);
        var findResult = await _productCollection.Find(filter).AnyAsync(cancellationToken);
        if (findResult)
        {
            await _productCollection.DeleteOneAsync(
                p => p.ProductId == productId,
                cancellationToken
            );
            return Result.Success();
        }

        return Result.Failure(ProductErrors.NotFound);
    }

    public async Task<Result<Product>> UpdateProduct(
        UpdateProduct.Command command,
        CancellationToken cancellationToken
    )
    {
        var value = command.UpdateProductRequest;
        var result = await GetProductById(command.ProductId, cancellationToken);
        var product = result.Value;
        product.UpdateProduct(
            value.Name,
            value.ProductCategory,
            value.Status,
            value.ProductVariants
        );
        return Result.Success(product);
    }

    public async Task<Result<Product>> GetProductById(
        string productId,
        CancellationToken cancellationToken
    )
    {
        var filter = Builders<Product>.Filter.Eq(p => p.ProductId, productId);
        if (
            await _productCollection.Find(filter).FirstOrDefaultAsync(cancellationToken) is
            { } product
        )
        {
            return Result.Success(product);
        }

        return Result.Failure<Product>(ProductErrors.NotFound);
    }
}
