namespace Client_App.Interfaces;

public interface IProductService<TResponse> : IApiService<TResponse>
{
    public Task<List<TResponse>> GetProductsByCategoryIdAsync(
        int offset,
        int categoryId,
        int limit = 10
    );
}
