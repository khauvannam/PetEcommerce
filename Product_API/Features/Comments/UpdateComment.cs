using BaseDomain.Results;
using MediatR;
using Product_API.Domains.Comments;
using Product_API.Interfaces;

namespace Product_API.Features.Comments;

public static class UpdateComment
{
    public sealed record Command(int CommentId, UpdateCommentRequest Request) : IRequest<Result>;

    public sealed class Handler(ICommentRepository repository) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateCommentRequest = request.Request;

            var result = await repository.GetByIdAsync(request.CommentId);

            if (result.IsFailure)
            {
                return result;
            }

            var comment = result.Value;

            comment.Update(updateCommentRequest.Rating, updateCommentRequest.Content);

            return await repository.UpdateAsync(comment);
        }
    }
}
