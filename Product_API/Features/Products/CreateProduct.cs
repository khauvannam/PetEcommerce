﻿using MediatR;
using Product_API.Domain;
using Shared.Domain.Results;

namespace Product_API.Features.Products;

public static class CreateProduct
{
    public class Command : IRequest<Result<Product>> { }

    public class Handler : IRequestHandler<Command, Result<Product>>
    {
        public Task<Result<Product>> Handle(Command request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
