namespace Client_App.Interfaces;

public interface IApiService<TResponse>
{
    public Task<List<TResponse>> GetAllAsync(int? limit = null, int? offset = null);
    public Task<TResponse> GetByIdAsync(string id);
    public Task CreateAsync(object item);
    public Task UpdateAsync(object item, string id);
    public Task DeleteAsync(string id);
}
