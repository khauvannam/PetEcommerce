using BaseDomain.Results;
using FluentValidation;
using MediatR;
using Product_API.Domains.Products;
using Product_API.Interfaces;

namespace Product_API.Features.Products;

public static class GetAllProducts
{
    public sealed record Query(Guid? CategoryId, int? Limit, int? Offset)
        : IRequest<Result<List<Product>>>;

    public sealed class Handler(IProductRepository repository, IValidator<Query> validator)
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
                return await repository.ListAllAsync(request);

            foreach (var error in validatorResult.Errors)
                result.AddResultList(new ErrorType("ListAllProduct.Query", error.ToString()));

            return result;
        }
    }

    public class Validator : AbstractValidator<Query>;
}
