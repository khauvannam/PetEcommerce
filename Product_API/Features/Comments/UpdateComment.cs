﻿using Base.Results;
using MediatR;
using Product_API.DTOs.Comments;
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

            comment.Update(
                updateCommentRequest.Rating,
                updateCommentRequest.Ttitle,
                updateCommentRequest.Content
            );

            return await repository.UpdateAsync(comment);
        }
    }
}
