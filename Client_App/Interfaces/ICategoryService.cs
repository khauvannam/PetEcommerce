namespace Client_App.Interfaces;

public interface ICategoryService<TResponse, TGetByIdResponse>
    : IApiService<TResponse, TGetByIdResponse>
{
    public Task<TGetByIdResponse> GetByEndpointAsync(string endpoint);
};
