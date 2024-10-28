using Client_App.Abstraction;
using Client_App.Components.Share.ReusableComponents;
using Client_App.DTOs.Products.Requests;
using Client_App.DTOs.Products.Responses;
using Client_App.DTOs.Share;
using Client_App.Interfaces;

namespace Client_App.Services.APIs;

public sealed class ProductService(
    IHttpClientFactory factory,
    string baseUrl = nameof(ProductService),
    string endpoint = "api/product"
) : ApiService(factory, baseUrl, endpoint), IProductService
{
    public async Task<Result<Pagination<T>>> GetProductsByConditionAsync<T>(
        int offset,
        int? categoryId,
        bool isBestSeller,
        int limit = 10
    )
        where T : ProductsInList
    {
        if (categoryId is null && !isBestSeller)
        {
            var products = await GetAllAsync<T>(limit, offset);
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

        var baseEndpoint = $"{Endpoint}{query}";

        var result = await Client.GetAsync(baseEndpoint);

        return await HandleResponse<Pagination<T>>(result);
    }

    public async Task<Result<Pagination<T>>> GetProductsBySearch<T>(
        ProductsSearchFilterRequest request
    )
    {
        var endpointUrl =
            $"{Endpoint}/search?searchtext={request.SearchText}&limit={request.Limit}&offset={request.Offset}";

        if (request.MinPrice > 0M)
        {
            endpointUrl += $"&minPrice={request.MinPrice}";
        }

        if (request.MaxPrice is > 0M and < 10000M)
        {
            endpointUrl += $"&maxPrice={request.MaxPrice}";
        }

        if (!string.IsNullOrEmpty(request.FilerBy))
        {
            endpointUrl += $"&filterBy={request.FilerBy}&isDesc={request.IsDesc}";
        }

        if (!request.Available)
        {
            endpointUrl += "&available=false";
        }

        var result = await Client.GetAsync(endpointUrl);
        return await HandleResponse<Pagination<T>>(result);
    }
};
