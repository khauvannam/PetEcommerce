using Client_App.Abstraction;
using Client_App.Domains.Products.Responses;
using Client_App.Interfaces;

namespace Client_App.Services;

public sealed class ProductService(
    IHttpClientFactory factory,
    string baseUrl = nameof(ProductService),
    string endpoint = "api/product"
) : ApiService<ProductModel>(factory, baseUrl, endpoint), IProductService<ProductModel>
{
    private readonly string _endpoint = endpoint;

    public async Task<List<ProductModel>> GetProductsByConditionAsync(
        int offset,
        int? categoryId,
        bool isBestSeller,
        int limit = 10
    )
    {
        if (categoryId is null && !isBestSeller)
        {
            var products = await GetAllAsync(limit, offset);
            return products;
        }

        var query = $"?limit={limit}&offset={offset}";
        if (categoryId is not null)
        {
            query += $"&categoryId={categoryId}";
        }

        if (isBestSeller)
        {
            query += $"&isBestSeller={isBestSeller}";
        }

        var baseEndpoint = $"{_endpoint}{query}";

        var result = await Client.GetAsync(baseEndpoint);

        return await HandleResponse<List<ProductModel>>(result) ?? [];
    }
};
