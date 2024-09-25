namespace Client_App.Interfaces;

public interface ICategoryService<TResponse> : IApiService<TResponse>
{
    public Task<TResponse> GetByEndpointAsync(string endpoint);
};
