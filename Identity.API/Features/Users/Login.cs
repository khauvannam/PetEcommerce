using FluentValidation;
using Identity.API.Common;
using Identity.API.Entities;
using Identity.API.Interfaces;
using MediatR;
using Shared.Shared;

namespace Identity.API.Features.Users;

public static class Login
{
    public record Command(string? Email, string? Password) : IRequest<Result<LoginResponseDto>>;

    internal sealed class Handler(IUserRepository repository)
        : IRequestHandler<Command, Result<LoginResponseDto>>
    {
        public async Task<Result<LoginResponseDto>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            return await repository.Login(request);
        }
    }

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Email).NotEmpty().WithMessage(ValidatorMessage.NotEmpty("email"));
            RuleFor(c => c.Password).NotEmpty().WithMessage(ValidatorMessage.NotEmpty("password"));
        }
    }
}
