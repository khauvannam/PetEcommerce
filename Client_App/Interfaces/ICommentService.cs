using Client_App.DTOs.Comments.Responses;
using Client_App.DTOs.Share;

namespace Client_App.Interfaces;

public interface ICommentService : IApiService
{
    public Task<Result<Pagination<T>>> GetAllByProductIdAsync<T>(
        int limit,
        int offset,
        int? productId
    )
        where T : Comment;
}
