namespace Client_App.Interfaces;

public interface IProductService<TResponse> : IApiService<TResponse>
{
    public Task<List<TResponse>> GetProductsByConditionAsync(
        int offset,
        int? categoryId,
        bool isBestSeller,
        int limit = 10
    );
}
