﻿using Base.Results;
using FluentValidation;
using Identity.API.Domains.Users;
using Identity.API.DTOs.Users;
using Identity.API.Helpers;
using Identity.API.Interfaces;
using MediatR;

namespace Identity.API.Features.Users;

public static class Login
{
    public record Command(string Email, string Password) : IRequest<Result<LoginResponse>>;

    internal sealed class Handler(IUserRepository repository, IValidator<Command> validator)
        : IRequestHandler<Command, Result<LoginResponse>>
    {
        public async Task<Result<LoginResponse>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validatorResult.IsValid)
            {
                var errors = string.Join(", ", validatorResult.Errors.Select(x => x.ErrorMessage));
                return Result.Failure<LoginResponse>(
                    new(nameof(Command), $"Invalid request : {errors}")
                );
            }

            return await repository.Login(request);
        }
    }

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Email).NotEmpty().WithMessage(ValidatorMessage.NotEmpty("email"));
            RuleFor(c => c.Password).NotEmpty().WithMessage(ValidatorMessage.NotEmpty("password"));
        }
    }
}
