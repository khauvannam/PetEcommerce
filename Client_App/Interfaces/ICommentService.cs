using Client_App.Domains.Comments.Responses;
using Client_App.Domains.Share;

namespace Client_App.Interfaces;

public interface ICommentService : IApiService
{
    public Task<Pagination<T>> GetAllByProductIdAsync<T>(int limit, int offset, Guid? productId)
        where T : Comment;
}
