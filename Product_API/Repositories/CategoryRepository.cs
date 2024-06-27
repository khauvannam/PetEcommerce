using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Product_API.Databases;
using Product_API.Domain.Categories;
using Product_API.Errors;
using Product_API.Features.Categories;
using Product_API.Interfaces;
using Shared.Domain.Results;

namespace Product_API.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IMongoCollection<Category> _categoryCollection;

    public CategoryRepository(IOptions<CategoryDatabaseSetting> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var database = client.GetDatabase(options.Value.DatabaseName);
        _categoryCollection = database.GetCollection<Category>(options.Value.CollectionName);
    }

    public async Task<Result> CreateCategory(CreateCategory.Command command)
    {
        var category = Category.Create(command.CategoryName, command.Details);
        await _categoryCollection.InsertOneAsync(category);
        return Result.Success();
    }

    public async Task<Result> DeleteCategory(DeleteCategory.Command command)
    {
        var filter = Builders<Category>.Filter.Eq(c => c.CategoryId, command.CategoryId);
        await _categoryCollection.DeleteOneAsync(filter);
        return Result.Success();
    }

    public async Task<Result<Category>> GetCategoryById(GetCategoryById.Command command)
    {
        var filter = Builders<Category>.Filter.Eq(c => c.CategoryId, command.CategoryId);
        if (await _categoryCollection.Find(filter).FirstOrDefaultAsync() is { } category)
        {
            return Result.Success(category);
        }

        return Result.Failure<Category>(CategoryErrors.NotFound);
    }

    public async Task<Result<List<Category>>> GetAllCategories()
    {
        var categories = await _categoryCollection.Find(_ => true).ToListAsync();
        return Result.Success(categories);
    }
}
