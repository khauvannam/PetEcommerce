using Client_App.Abstraction;
using Client_App.DTOs.Comments.Responses;
using Client_App.DTOs.Share;
using Client_App.Interfaces;

namespace Client_App.Services.APIs;

public class CommentService(
    IHttpClientFactory factory,
    string baseUrl = nameof(ProductService),
    string endpoint = "api/comment"
) : ApiService(factory, baseUrl, endpoint), ICommentService
{
    public async Task<Result<Pagination<T>>> GetAllByProductIdAsync<T>(
        int limit,
        int offset,
        int? productId
    )
        where T : Comment
    {
        if (productId is null)
        {
            return await GetAllAsync<T>(limit, offset);
        }

        var query = "?limit=" + limit + "&offset=" + offset + "&productId=" + productId;
        var result = await Client.GetAsync($"{Endpoint}{query}");

        return await HandleResponse<Pagination<T>>(result);
    }
}
