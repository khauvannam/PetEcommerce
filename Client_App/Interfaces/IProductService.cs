namespace Client_App.Interfaces;

public interface IProductService<TResponse, TGetByIdResponse>
    : IApiService<TResponse, TGetByIdResponse>
{
    public Task<List<TResponse>> GetProductsByConditionAsync(
        int offset,
        int? categoryId,
        bool isBestSeller,
        int limit = 10
    );
}
