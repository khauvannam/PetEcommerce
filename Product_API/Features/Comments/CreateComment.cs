using Base.Results;
using MediatR;
using Product_API.Domain.Comments;
using Product_API.Interfaces;

namespace Product_API.Features.Comments;

public static class CreateComment
{
    public sealed record Command(
        int BuyerId,
        string BuyerEmail,
        int ProductId,
        int Rating,
        string Title,
        string Content
    ) : IRequest<Result>;

    public sealed class Handler(ICommentRepository repository) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var comment = Comment.Create(
                request.BuyerId,
                request.BuyerEmail,
                request.Rating,
                request.Title,
                request.Content
            );
            comment.AssignProduct(request.ProductId);
            var result = await repository.CreateAsync(comment);
            return result;
        }
    }
}
