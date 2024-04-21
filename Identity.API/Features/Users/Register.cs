using FluentValidation;
using Identity.API.Common;
using Identity.API.Interfaces;
using MediatR;
using Shared.Common;
using Shared.Shared;

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
    }

    internal sealed class Handler(IUserRepository repository, IValidator<Command> validator)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var validateResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validateResult.IsValid)
            {
                return Result.Failure(
                    new ErrorType("Register.Command", validateResult.Errors.ToString()!)
                );
            }
            return await repository.Register(request);
        }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(u => u.Email).EmailAddress().WithMessage("Your email isn't accepted");
            RuleFor(u => u.PhoneNumber).Length(11).WithMessage("This is not a valid phone number");
            RuleFor(u => u.Password)
                .NotEmpty()
                .GreaterThan("8")
                .MaximumLength(15)
                .WithMessage("Password have to be at least 8 characters and maximum 15 characters");
        }
    }
}
