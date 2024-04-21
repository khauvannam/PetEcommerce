using FluentValidation;
using Identity.API.Common;
using Identity.API.Interfaces;
using MediatR;
using Shared.Shared;

namespace Identity.API.Features.Users;

public static class ResetPassword
{
    public record Command(string Email, string Token, string Password, string ConfirmPassword)
        : IRequest<Result>;

    internal sealed class Handler(IUserRepository repository) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            return await repository.ResetPassword(request);
        }
    }

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Token).NotEmpty().WithMessage(ValidatorMessage.NotEmpty("Reset code"));

            RuleFor(c => c.Password).NotEmpty().WithMessage(ValidatorMessage.NotEmpty("Password"));

            RuleFor(c => c.ConfirmPassword)
                .NotEmpty()
                .WithMessage(ValidatorMessage.NotEmpty("Confirm Password"))
                .Equal(c => c.Password)
                .WithMessage("Password and Confirm Password have to be the same");
        }
    }
}
