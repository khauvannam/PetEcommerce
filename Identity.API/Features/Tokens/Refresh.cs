using FluentValidation;
using Identity.API.Entities;
using Identity.API.Interfaces;
using MediatR;
using Shared.Shared;

namespace Identity.API.Features.Tokens;

public static class Refresh
{
    public record Command(string RefreshToken, string AccessToken)
        : IRequest<Result<TokenResponseDto>>;

    internal class Handler(ITokenRepository repository)
        : IRequestHandler<Command, Result<TokenResponseDto>>
    {
        public Task<Result<TokenResponseDto>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            return repository.Refresh(request);
        }
    }

    internal class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.RefreshToken).NotEmpty();
            RuleFor(c => c.AccessToken).NotEmpty();
        }
    }
}
