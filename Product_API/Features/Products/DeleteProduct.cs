using BaseDomain.Results;
using MediatR;
using Product_API.Interfaces;
using Shared.Services;

namespace Product_API.Features.Products;

public static class DeleteProduct
{
    public record Command(Guid ProductId) : IRequest<Result>;

    public class Handler(IProductRepository repository, BlobService blobService)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await repository.GetByIdAsync(request.ProductId, cancellationToken);
            if (result.IsFailure)
                return result;

            var product = result.Value;
            var fileName = new Uri(product.ImageUrl).Segments[^1];
            await blobService.DeleteAsync(fileName);
            return await repository.DeleteAsync(product, cancellationToken);
        }
    }
}
