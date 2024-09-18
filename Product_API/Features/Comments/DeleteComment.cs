using BaseDomain.Results;
using MediatR;
using Product_API.Interfaces;

namespace Product_API.Features.Comments;

public static class DeleteComment
{
    public sealed record Command(int Id) : IRequest<Result>;

    public sealed class Handler(ICommentRepository repository) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await repository.GetByIdAsync(request.Id);
            if (result.IsFailure)
            {
                return result;
            }
            var comment = result.Value;

            return await repository.DeleteAsync(comment);
        }
    }
}
