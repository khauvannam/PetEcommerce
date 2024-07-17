using BaseDomain.Results;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Product_API.Databases;
using Product_API.Domains.Categories;
using Product_API.Errors;
using Product_API.Features.Categories;
using Product_API.Interfaces;
using Shared.Domain.Services;
using Shared.Services;
using StackExchange.Redis;

namespace Product_API.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly BlobService _blobService;
    private readonly IDatabase _database;
    private const string CategoryKeyPrefix = "Category-";

    public CategoryRepository(IOptions<CategoryDatabaseSetting> options, BlobService blobService)
    {
        _blobService = blobService;
        var redis = ConnectionMultiplexer.Connect(options.Value.ConnectionString);
        _database = redis.GetDatabase(options.Value.DatabaseIndex);
    }

    public async Task<Result> CreateCategory(CreateCategory.Command command)
    {
        var fileName = (await _blobService.UploadFileAsync(command.File, CategoryKeyPrefix)).Value;
        var category = Category.Create(command.CategoryName, command.Description, fileName);
        var categoryJson = JsonConvert.SerializeObject(category);
        await _database.StringSetAsync(CategoryKeyPrefix + category.CategoryId, categoryJson);
        return Result.Success();
    }

    public async Task<Result> DeleteCategory(DeleteCategory.Command command)
    {
        bool deleted = await _database.KeyDeleteAsync(CategoryKeyPrefix + command.CategoryId);
        return deleted ? Result.Success() : Result.Failure(CategoryErrors.NotFound);
    }

    public async Task<Result<Category>> GetCategoryById(GetCategoryById.Query command)
    {
        var categoryJson = await _database.StringGetAsync(CategoryKeyPrefix + command.CategoryId);
        if (!categoryJson.IsNullOrEmpty)
        {
            var category = JsonConvert.DeserializeObject<Category>(categoryJson!);
            return Result.Success(category)!;
        }
        return Result.Failure<Category>(CategoryErrors.NotFound);
    }

    public async Task<Result<List<Category>>> GetAllCategories()
    {
        var server = _database.Multiplexer.GetServer(_database.Multiplexer.GetEndPoints().First());
        var keys = server.Keys(pattern: CategoryKeyPrefix + "*").ToArray();
        var categories = new List<Category>();

        foreach (var key in keys)
        {
            var categoryJson = await _database.StringGetAsync(key);
            if (categoryJson.IsNullOrEmpty)
                continue;
            var category = JsonConvert.DeserializeObject<Category>(
                categoryJson!,
                new JsonSerializerSettings()
                {
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                    ContractResolver = new PrivateSetterJsonResolver()
                }
            );
            categories.Add(category!);
        }

        return Result.Success(categories);
    }
}
