using Client_App.Abstraction;
using Client_App.Domains.Products.Responses;
using Client_App.Interfaces;

namespace Client_App.Services;

public sealed class ProductService(
    IHttpClientFactory factory,
    string baseUrl = nameof(ProductService),
    string endpoint = "api/product"
) : ApiService<Product>(factory, baseUrl, endpoint), IProductService<Product>
{
    private readonly string _endpoint = endpoint;

    public async Task<List<Product>> GetProductsByCategoryIdAsync(
        int offset,
        int categoryId,
        int limit = 10
    )
    {
        var query = $"?limit={limit}&offset={offset}&categoryId={categoryId}";
        var baseEndpoint = $"{_endpoint}{query}";

        var result = await Client.GetAsync(baseEndpoint);

        return await HandleResponse<List<Product>>(result) ?? [];
    }
};
