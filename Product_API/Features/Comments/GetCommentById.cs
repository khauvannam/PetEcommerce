using Base.Results;
using MediatR;
using Product_API.Domain.Comments;
using Product_API.Interfaces;

namespace Product_API.Features.Comments;

public static class GetCommentById
{
    public sealed record Query(int Id) : IRequest<Result<Comment>>;

    public sealed class Handler(ICommentRepository repository)
        : IRequestHandler<Query, Result<Comment>>
    {
        public async Task<Result<Comment>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var result = await repository.GetByIdAsync(request.Id);
            return result;
        }
    }
}
