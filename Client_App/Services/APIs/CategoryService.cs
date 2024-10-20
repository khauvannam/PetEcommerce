using Client_App.Abstraction;
using Client_App.DTOs.Categories.Responses;
using Client_App.DTOs.Share;
using Client_App.Interfaces;

namespace Client_App.Services.APIs;

public class CategoryService(
    IHttpClientFactory clientFactory,
    string baseUrl = nameof(ProductService),
    string endpoint = "api/category"
) : ApiService(clientFactory, baseUrl, endpoint), ICategoryService
{
    private readonly string _uri = endpoint;

    public async Task<Result<T>> GetByEndpointAsync<T>(string endpoint)
        where T : Category
    {
        var baseEndpoint = $"{_uri}/{endpoint}";
        var result = await Client.GetAsync(baseEndpoint);
        return await HandleResponse<T>(result);
    }
}
