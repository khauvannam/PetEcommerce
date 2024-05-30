using FluentValidation;
using Identity.API.Common;
using Identity.API.Interfaces;
using MediatR;
using Shared.Domain.Results;

namespace Identity.API.Features.Users;

public static class ForgotPassword
{
    public record Command(string? Email) : IRequest<Result>;

    internal sealed class Handler(IUserRepository repository) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            return await repository.ForgotPassword(request);
        }
    }

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Email)
                .NotEmpty()
                .WithMessage(ValidatorMessage.MustBeUnique("email"))
                .EmailAddress()
                .WithMessage("Your Email is invalid");
        }
    }
}
