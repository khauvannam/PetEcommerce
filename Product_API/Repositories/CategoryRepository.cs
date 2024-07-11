using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Product_API.Databases;
using Product_API.Domain.Categories;
using Product_API.Errors;
using Product_API.Features.Categories;
using Product_API.Interfaces;
using Shared.Domain.Results;
using StackExchange.Redis;

namespace Product_API.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IDatabase _database;
    private const string CategoryKeyPrefix = "Category:";

    public CategoryRepository(IOptions<CategoryDatabaseSetting> options)
    {
        var redis = ConnectionMultiplexer.Connect(options.Value.ConnectionString);
        _database = redis.GetDatabase(options.Value.DatabaseIndex);
    }

    public async Task<Result> CreateCategory(CreateCategory.Command command)
    {
        HashSet<string> details = new();
        foreach (var detail in command.Details)
        {
            if (!details.Add(detail))
            {
                throw new ArgumentException("Detail cannot be duplicated");
            }
        }
        var category = Category.Create(command.CategoryName, details);
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
            if (!categoryJson.IsNullOrEmpty)
            {
                var category = JsonConvert.DeserializeObject<Category>(categoryJson!);
                categories.Add(category!);
            }
        }

        return Result.Success(categories);
    }
}
