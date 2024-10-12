using Client_App.Domains.Share;

namespace Client_App.Interfaces;

public interface IApiService
{
    Task<Pagination<T>> GetAllAsync<T>(int? limit = default, int? offset = default)
        where T : class;
    Task<T> GetByIdAsync<T>(Guid id)
        where T : class;
    Task CreateAsync<TRequest>(TRequest item);
    Task UpdateAsync<TRequest>(TRequest item, string id);
    Task DeleteAsync(string id);
}
