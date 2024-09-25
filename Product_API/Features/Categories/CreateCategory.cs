using BasedDomain.Results;
using MediatR;
using Product_API.Domains.Categories;
using Product_API.Interfaces;
using Shared.Services;

namespace Product_API.Features.Categories;

public static class CreateCategory
{
    public sealed record Command(string CategoryName, string Description, IFormFile File)
        : IRequest<Result>;

    public sealed class Handler(ICategoryRepository repository, BlobService blobService)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var fileName = await blobService.UploadFileAsync(request.File, "Category-");
            var category = Category.Create(request.CategoryName, request.Description, fileName);

            return await repository.CreateAsync(category);
        }
    }
}
