using Client_App.DTOs.Share;

namespace Client_App.Interfaces;

public interface IApiService
{
    Task<Result<Pagination<T>>> GetAllAsync<T>(int? limit = default, int? offset = default)
        where T : class;

    Task<Result<T>> GetByIdAsync<T>(int id)
        where T : class;

    Task<Result> CreateAsync<TRequest>(TRequest item)
        where TRequest : class;

    Task<Result> UpdateAsync<TRequest>(TRequest item, int id)
        where TRequest : class;

    Task DeleteAsync(int id);
}
