using Base.Results;
using FluentValidation;
using Identity.API.Domains.Tokens;
using Identity.API.DTOs.Tokens;
using Identity.API.Interfaces;
using MediatR;

namespace Identity.API.Features.Tokens;

public static class Refresh
{
    public record Command(string RefreshToken, string AccessToken)
        : IRequest<Result<TokenResponse>>;

    internal class Handler(ITokenRepository repository)
        : IRequestHandler<Command, Result<TokenResponse>>
    {
        public Task<Result<TokenResponse>> Handle(
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
