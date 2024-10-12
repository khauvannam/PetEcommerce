using Base.Results;
using FluentValidation;
using Identity.API.Domains.Users;
using Identity.API.Helpers;
using Identity.API.Interfaces;
using MediatR;

namespace Identity.API.Features.Users;

public static class Register
{
    public sealed class Command : IRequest<Result>
    {
        public required string Email { get; init; }
        public required string Username { get; init; }
        public required string Password { get; init; }
        public required string PhoneNumber { get; init; }
        public required Address Address { get; init; }
    }

    public sealed class Handler(IUserRepository repository, IValidator<Command> validator)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var validateResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validateResult.IsValid)
            {
                var errors = string.Join(", ", validateResult.Errors.Select(x => x.ErrorMessage));
                return Result.Failure(new(nameof(Command), $"Invalid request : {errors}"));
            }

            var user = new User
            {
                Email = request.Email,
                UserName = request.Username,
                SecurityStamp = Guid.NewGuid().ToString(),
                PhoneNumber = request.PhoneNumber,
            };

            var city = request.Address.City;
            var street = request.Address.Street;
            var zipCode = request.Address.ZipCode;

            var address = Address.Create(street, city, zipCode);
            user.UpdateAddress(address);

            return await repository.Register(user, request.Password);
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
