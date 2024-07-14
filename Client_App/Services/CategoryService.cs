using System.Text.Json;
using Client_App.Domains.Categories;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Client_App.Services;

public class CategoryService(IHttpClientFactory clientFactory)
{
    private readonly HttpClient _client = clientFactory.CreateClient(nameof(ProductService));

    public async Task<List<Category>> GetCategories()
    {
        var result = await _client.GetAsync("api/categories");
        var content = await result.Content.ReadAsStringAsync();
        var categories = JsonSerializer.Deserialize<List<Category>>(
            content,
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
        )!;
        return categories;
    }
}
