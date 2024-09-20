using Client_App.Abstraction;
using Client_App.Domains.Categories.Responses;

namespace Client_App.Services;

public class CategoryService(
    IHttpClientFactory clientFactory,
    string baseUrl = nameof(ProductService),
    string endpoint = "api/category"
) : ApiService<Category>(clientFactory, baseUrl, endpoint);
