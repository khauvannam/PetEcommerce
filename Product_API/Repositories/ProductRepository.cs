﻿using BaseDomain.Results;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Product_API.Databases;
using Product_API.Domains.Products;
using Product_API.Errors;
using Product_API.Features.Products;
using Product_API.Interfaces;
using Shared.Domain.Services;
using Shared.Services;
using StackExchange.Redis;

namespace Product_API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly BlobService _blobService;
    private readonly IDatabase _database;
    private const string ProductKeyPrefix = "Product-";

    public ProductRepository(IOptions<ProductDatabaseSetting> options, BlobService blobService)
    {
        _blobService = blobService;
        var redis = ConnectionMultiplexer.Connect(options.Value.ConnectionString);
        _database = redis.GetDatabase(options.Value.DatabaseIndex);
    }

    public async Task<Result<List<Product>>> ListAllProducts(ListAllProducts.Query command)
    {
        var server = _database.Multiplexer.GetServer(_database.Multiplexer.GetEndPoints().First());
        var keys = server.Keys(pattern: ProductKeyPrefix + "*").ToArray();
        var products = new List<Product>();

        foreach (var key in keys.Skip(command.Offset).Take(command.Limit))
        {
            var productJson = await _database.StringGetAsync(key);
            if (!productJson.IsNullOrEmpty)
            {
                var product = JsonConvert.DeserializeObject<Product>(
                    productJson!,
                    new JsonSerializerSettings
                    {
                        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                        ContractResolver = new PrivateSetterJsonResolver()
                    }
                )!;
                if (
                    command.CategoryId == null
                    || product.ProductCategory.ProductCategoryId == command.CategoryId
                )
                {
                    products.Add(product);
                }
            }
        }

        return Result.Success(products);
    }

    public async Task<Result<Product>> CreateProduct(CreateProduct.Command command)
    {
        var productCategory = ProductCategory.Create(
            command.ProductCategory.ProductCategoryId,
            command.ProductCategory.Details
        );
        var fileName = (await _blobService.UploadFileAsync(command.File, ProductKeyPrefix)).Value;
        var product = Product.Create(
            command.Name,
            command.Description,
            command.ProductUseGuide,
            fileName,
            productCategory
        );

        foreach (var variant in command.ProductVariants)
        {
            var productVariant = ProductVariant.Create(variant.VariantName, variant.InStock);
            productVariant.SetPrice(variant.OriginalPrice);
            productVariant.ApplyDiscount(variant.DiscountPercent);
            product.AddProductVariants(productVariant);
        }

        var productJson = JsonConvert.SerializeObject(product);
        await _database.StringSetAsync(ProductKeyPrefix + product.ProductId, productJson);
        return Result.Success(product);
    }

    public async Task<Result> DeleteProduct(string productId, CancellationToken cancellationToken)
    {
        var deleted = await _database.KeyDeleteAsync(ProductKeyPrefix + productId);
        return deleted ? Result.Success() : Result.Failure(ProductErrors.NotFound);
    }

    public async Task<Result<Product>> UpdateProduct(
        UpdateProduct.Command command,
        CancellationToken cancellationToken
    )
    {
        var productJson = await _database.StringGetAsync(ProductKeyPrefix + command.ProductId);
        if (productJson.IsNullOrEmpty)
        {
            return Result.Failure<Product>(ProductErrors.NotFound);
        }
        var product = JsonConvert.DeserializeObject<Product>(
            productJson!,
            new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver = new PrivateSetterJsonResolver(),
            }
        )!;
        var value = command.UpdateProductRequest;
        var fileName = (await _blobService.UploadFileAsync(value.File, ProductKeyPrefix)).Value;
        product.UpdateProduct(
            value.Name,
            value.Description,
            value.ProductUseGuide,
            fileName,
            value.ProductCategory,
            value.ProductVariants
        );

        productJson = JsonConvert.SerializeObject(product);
        await _database.StringSetAsync(ProductKeyPrefix + product.ProductId, productJson);
        return Result.Success(product);
    }

    public async Task<Result<Product>> GetProductById(
        string productId,
        CancellationToken cancellationToken
    )
    {
        var productJson = await _database.StringGetAsync(ProductKeyPrefix + productId);
        if (productJson.IsNullOrEmpty)
        {
            return Result.Failure<Product>(ProductErrors.NotFound);
        }
        var product = JsonConvert.DeserializeObject<Product>(productJson!)!;
        return Result.Success(product);
    }
}
