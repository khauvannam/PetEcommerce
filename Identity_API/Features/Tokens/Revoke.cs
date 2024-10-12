using Base.Results;
using FluentValidation;
using Identity.API.Interfaces;
using MediatR;

namespace Identity.API.Features.Tokens;

public static class Revoke
{
    public record Command(Guid UserId) : IRequest<Result>;

    internal class Handler(ITokenRepository repository) : IRequestHandler<Command, Result>
    {
        public Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            return repository.Revoke(request);
        }
    }

    internal class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.UserId).NotEmpty().NotNull();
        }
    }
}
