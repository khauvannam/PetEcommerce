using BaseDomain.Results;
using MediatR;
using Product_API.Interfaces;

namespace Product_API.Features.Discounts;

public static class GetAllDiscount
{
    public record Query : IRequest<Result>;

    public class Handler(IDiscountRepository repository) : IRequestHandler<Query, Result>
    {
        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            return await repository.GetAllAsync();
        }
    }
}
