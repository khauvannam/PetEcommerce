using Base.Results;
using Basket_API.Domain.Baskets;
using Basket_API.Interfaces;
using MediatR;

namespace Basket_API.Features.Baskets;

public static class GetBasketById
{
    public record Query(int BasketId) : IRequest<Result<Basket>>;

    internal sealed class Handler(IBasketRepository repository)
        : IRequestHandler<Query, Result<Basket>>
    {
        public async Task<Result<Basket>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await repository.GetByIdAsync(request.BasketId);
        }
    }
}
