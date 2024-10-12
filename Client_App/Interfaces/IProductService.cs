using Client_App.Domains.Products.Responses;
using Client_App.Domains.Share;

namespace Client_App.Interfaces;

public interface IProductService : IApiService
{
    public Task<Pagination<T>> GetProductsByConditionAsync<T>(
        int offset,
        int? categoryId,
        bool isBestSeller,
        int limit = 10
    )
        where T : ProductsInList;
}
