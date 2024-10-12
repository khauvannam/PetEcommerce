using Base;
using Base.Results;
using Product_API.Domains.Comments;
using Product_API.Domains.Discounts;

namespace Product_API.Interfaces;

public interface ICommentRepository
{
    public Task<Result> CreateAsync(Comment comment);
    public Task<Result> DeleteAsync(Comment comment);
    public Task<Result> UpdateAsync(Comment comment);
    public Task<Result<Comment>> GetByIdAsync(int id);

    public Task<Result<Pagination<Comment>>> GetAllAsync(int limit, int offset, Guid? productId);
}
