using BaseDomain.Results;
using Product_API.Domains.Comments;
using Product_API.Interfaces;

namespace Product_API.Repositories;

public class CommentRepository : ICommentRepository
{
    public Task<Result> CreateAsync(Comment comment)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(Comment comment)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAsync(Comment comment)
    {
        throw new NotImplementedException();
    }

    public Task<Comment?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Comment>> GetAllAsync(Guid? productId)
    {
        throw new NotImplementedException();
    }
}
