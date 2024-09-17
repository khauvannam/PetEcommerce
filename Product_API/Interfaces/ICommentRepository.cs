﻿using BaseDomain.Results;
using Product_API.Domains.Comments;
using Product_API.Domains.Discounts;

namespace Product_API.Interfaces;

public interface ICommentRepository
{
    public Task<Result> CreateAsync(Comment comment);
    public Task<Result> DeleteAsync(Comment comment);
    public Task<Result> UpdateAsync(Comment comment);
    public Task<Comment?> GetByIdAsync(Guid id);
    public Task<List<Comment>> GetAllAsync(Guid? productId);
}
