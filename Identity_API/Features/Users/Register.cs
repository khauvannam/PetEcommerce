using FluentValidation;
using Identity.API.Common;
using Identity.API.Domain.Users;
using Identity.API.Interfaces;
using MediatR;
using Shared.Domain.Results;

namespace Identity.API.Features.Users;

public static class Register
{
    public sealed class Command : IRequest<Result>
    {
        public string? Email { get; init; }
        public string? Username { get; init; }
        public string? Password { get; init; }
        public string? PhoneNumber { get; init; }
        public string SecurityStamp { get; init; } = Guid.NewGuid().ToString();
        public Address Address { get; init; } = null!;
    }

    internal sealed class Handler(IUserRepository repository, IValidator<Command> validator)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var validateResult = await validator.ValidateAsync(request, cancellationToken);

            if (validateResult.IsValid)
            {
                return await repository.Register(request);
            }

            var result = Result.Create(false);
            foreach (var error in validateResult.Errors)
            {
                result.AddResultList(new("Register.Command", error.ToString()));
            }

            return result;
        }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(IUserServiceRepository repository)
        {
            RuleFor(u => u.Email).EmailAddress().WithMessage("Your email isn't accepted");

            RuleFor(u => u.PhoneNumber).NotEmpty().WithMessage("This is not a valid phone number");

            RuleFor(u => u.Password)
                .NotEmpty()
                .GreaterThan("8")
                .MaximumLength(15)
                .WithMessage("Password have to be at least 8 characters and maximum 15 characters");

            RuleFor(c => c.Email)
                .MustAsync(
                    async (email, _) =>
                    {
                        var result = await repository.IsEmailUniqueAsync(email!);
                        return result.Value;
                    }
                )
                .WithMessage(ValidatorMessage.MustBeUnique("Email"));

            RuleFor(c => c.Username)
                .MustAsync(
                    async (username, _) =>
                    {
                        var result = await repository.IsUsernameUniqueAsync(username!);
                        return result.Value;
                    }
                )
                .WithMessage(ValidatorMessage.MustBeUnique("Username"));
        }
    }
}
