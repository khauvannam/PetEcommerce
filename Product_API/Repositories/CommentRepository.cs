using BaseDomain.Results;
using Microsoft.EntityFrameworkCore;
using Product_API.Databases;
using Product_API.Domains.Comments;
using Product_API.Errors;
using Product_API.Interfaces;

namespace Product_API.Repositories;

public class CommentRepository(ProductDbContext dbContext) : ICommentRepository
{
    public async Task<Result> CreateAsync(Comment comment)
    {
        dbContext.Comments.Add(comment);
        await dbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Comment comment)
    {
        dbContext.Comments.Remove(comment);
        await dbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateAsync(Comment comment)
    {
        await dbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<Comment>> GetByIdAsync(int id)
    {
        var comment = await dbContext.Comments.FirstOrDefaultAsync(c => c.CommentId == id);
        if (comment is null)
        {
            return Result.Failure<Comment>(CommentErrors.NotFound);
        }
        return Result.Success(comment);
    }

    public async Task<Result<List<Comment>>> GetAllAsync(Guid? productId)
    {
        var query = dbContext.Comments.AsQueryable().AsNoTracking();
        if (productId is not null)
        {
            query = query.Where(c => c.ProductId == productId);
        }
        var comments = await query.ToListAsync();
        return Result.Success(comments);
    }
}
