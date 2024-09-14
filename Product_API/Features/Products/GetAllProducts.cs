﻿using BaseDomain.Results;
using FluentValidation;
using MediatR;
using Product_API.Domains.Products;
using Product_API.Interfaces;

namespace Product_API.Features.Products;

public static class GetAllProducts
{
    public record Query(int? Limit = 10, int? Offset = 0, Guid? CategoryId = null)
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
                return await repository.ListAllAsync(request);

            foreach (var error in validatorResult.Errors)
                result.AddResultList(new ErrorType("ListAllProduct.Query", error.ToString()));

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
}
