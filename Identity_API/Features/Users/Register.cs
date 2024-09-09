using BaseDomain.Results;
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
        public string? Email { get; init; }
        public string? Username { get; init; }
        public string Password { get; init; } = null!;
        public string? PhoneNumber { get; init; }
        public string SecurityStamp { get; } = Guid.NewGuid().ToString();
        public Address? Address { get; init; }
    }

    public sealed class Handler(IUserRepository repository, IValidator<Command> validator)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var validateResult = await validator.ValidateAsync(request, cancellationToken);

            var user = new User
            {
                Email = request.Email,
                UserName = request.Username,
                SecurityStamp = request.SecurityStamp,
                PhoneNumber = request.PhoneNumber,
            };

            var city = request.Address?.City;
            var street = request.Address?.Street;
            var zipCode = request.Address?.ZipCode;

            if (street != null && city != null && zipCode != null)
            {
                var address = Address.Create(street, city, zipCode);
                user.UpdateAddress(address);
            }

            if (validateResult.IsValid)
                return await repository.Register(user, request.Password);

            var result = Result.Create();

            foreach (var error in validateResult.Errors)
                result.AddResultList(new ErrorType("Register.Command", error.ToString()));

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
