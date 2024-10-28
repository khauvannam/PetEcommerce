using Client_App.DTOs.Products.Requests;
using Client_App.DTOs.Products.Responses;
using Client_App.DTOs.Share;

namespace Client_App.Interfaces;

public interface IProductService : IApiService
{
    Task<Result<Pagination<T>>> GetProductsByConditionAsync<T>(
        int offset,
        int? categoryId,
        bool isBestSeller,
        int limit = 10
    )
        where T : ProductsInList;

    Task<Result<Pagination<T>>> GetProductsBySearch<T>(ProductsSearchFilterRequest request);
}
