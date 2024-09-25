using Client_App.Abstraction;
using Client_App.Domains.Categories.Responses;
using Client_App.Interfaces;

namespace Client_App.Services;

public class CategoryService(
    IHttpClientFactory clientFactory,
    string baseUrl = nameof(ProductService),
    string endpoint = "api/category"
) : ApiService<Category>(clientFactory, baseUrl, endpoint), ICategoryService<Category>
{
    private readonly string _uri = endpoint;

    public async Task<Category> GetByEndpointAsync(string endpoint)
    {
        var baseEndpoint = $"{_uri}/{endpoint}";
        var result = await Client.GetAsync(baseEndpoint);
        return (await HandleResponse<Category>(result))!;
    }
}
