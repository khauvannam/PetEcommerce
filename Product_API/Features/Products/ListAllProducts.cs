using Carter;
using FluentValidation;
using MediatR;
using Product_API.Domain.Products;
using Product_API.Interfaces;
using Shared.Domain.Results;

namespace Product_API.Features.Products;

public static class ListAllProducts
{
    public record Query(int Limit = 10, int Offset = 0, string? CategoryId = null)
        : IRequest<Result<List<Product>>>;

    public class Handler(IProductRepository repository, IValidator<Query> validator)
        : IRequestHandler<Query, Result<List<Product>>>
    {
        public async Task<Result<List<Product>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);
            var result = Result.Create<List<Product>>(false);
            if (validatorResult.IsValid)
            {
                return await repository.ListAllProducts(request);
            }

            foreach (var error in validatorResult.Errors)
            {
                result.AddResultList(new("ListAllProduct.Query", error.ToString()));
            }

            return result;
        }
    }

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(c => c.Limit).GreaterThan(0).WithMessage("Limit have to at least 1");
            RuleFor(c => c.Offset).GreaterThan(-1).WithMessage("Offset have to greater than 0");
        }
    }

    public class Endpoint : CarterModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/product",
                async (ISender sender, Query query) =>
                {
                    var result = await sender.Send(query);
                    if (result.IsFailure)
                    {
                        return Results.NotFound(result.ErrorTypes);
                    }

                    return Results.Ok(result);
                }
            );
        }
    }
}
