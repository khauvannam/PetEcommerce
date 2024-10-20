using Client_App.DTOs.Categories.Responses;
using Client_App.DTOs.Share;

namespace Client_App.Interfaces;

public interface ICategoryService : IApiService
{
    Task<Result<T>> GetByEndpointAsync<T>(string categoryEndpoint)
        where T : Category;
}
