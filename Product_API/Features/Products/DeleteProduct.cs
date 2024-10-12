using Base.Results;
using MediatR;
using Product_API.Interfaces;
using Shared.Services;

namespace Product_API.Features.Products;

public static class DeleteProduct
{
    public sealed record Command(Guid ProductId) : IRequest<Result>;

    public sealed class Handler(IProductRepository repository, BlobService blobService)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await repository.GetByIdAsync(request.ProductId, cancellationToken);
            if (result.IsFailure)
            {
                return result;
            }

            var product = result.Value;
            foreach (
                var fileName in product.ImageUrlList.Select(img => new Uri(img).Segments.Last())
            )
            {
                await blobService.DeleteAsync(fileName);
            }
            return await repository.DeleteAsync(product, cancellationToken);
        }
    }
}
