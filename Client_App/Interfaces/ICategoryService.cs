using Client_App.Domains.Categories.Responses;

namespace Client_App.Interfaces;

public interface ICategoryService : IApiService
{
    Task<T> GetByEndpointAsync<T>(string categoryEndpoint)
        where T : Category;
}
