using Base.Results;
using MediatR;
using Product_API.Interfaces;

namespace Product_API.Features.Discounts;

public static class GetAllDiscount
{
    public class Query : IRequest<Result>;

    public class Handler(IDiscountRepository repository) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            return await repository.GetAllAsync();
        }
    }
}
