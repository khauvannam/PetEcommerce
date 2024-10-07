using BasedDomain;
using Client_App.Domains.Products.Responses;

namespace Client_App.Interfaces;

public interface IProductService<TResponse, TGetByIdResponse>
    : IApiService<TResponse, TGetByIdResponse>
{
    public Task<Pagination<ProductsInList>> GetProductsByConditionAsync(
        int offset,
        int? categoryId,
        bool isBestSeller,
        int limit = 10
    );
}
