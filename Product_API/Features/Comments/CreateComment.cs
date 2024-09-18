using BaseDomain.Results;
using MediatR;
using Product_API.Domains.Comments;
using Product_API.Interfaces;

namespace Product_API.Features.Comments;

public static class CreateComment
{
    public sealed record Command(Guid BuyerId, int Rating, string Content) : IRequest<Result>;

    public sealed class Handler(ICommentRepository repository) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var comment = Comment.Create(request.BuyerId, request.Rating, request.Content);
            var result = await repository.CreateAsync(comment);
            return result;
        }
    }
}
