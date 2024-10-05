namespace Client_App.Interfaces;

public interface IApiService<TResponse, TGetByIdResponse>
{
    public Task<List<TResponse>> GetAllAsync(int? limit = null, int? offset = null);
    public Task<TGetByIdResponse> GetByIdAsync(Guid id);
    public Task CreateAsync(object item);
    public Task UpdateAsync(object item, string id);
    public Task DeleteAsync(string id);
}
