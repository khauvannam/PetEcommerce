using Base;
using Base.Results;
using MediatR;
using Product_API.Domain.Comments;
using Product_API.Interfaces;

namespace Product_API.Features.Comments;

public static class GetAllComment
{
    public sealed record Query(int Limit, int Offset, int? ProductId)
        : IRequest<Result<Pagination<Comment>>>;

    public sealed class Handler(ICommentRepository repository)
        : IRequestHandler<Query, Result<Pagination<Comment>>>
    {
        public async Task<Result<Pagination<Comment>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var result = await repository.GetAllAsync(
                request.Limit,
                request.Offset,
                request.ProductId
            );
            return result;
        }
    }
}
