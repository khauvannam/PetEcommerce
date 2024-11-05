using Base.Results;
using MediatR;
using Product_API.Domain.Categories;
using Product_API.Interfaces;
using Share.Services;

namespace Product_API.Features.Categories;

public static class UpdateCategory
{
    public sealed record Command(
        int CategoryId,
        string CategoryName,
        string Description,
        IFormFile? File
    ) : IRequest<Result<Category>>;

    public sealed class Handler(ICategoryRepository repository, BlobService blobService)
        : IRequestHandler<Command, Result<Category>>
    {
        public async Task<Result<Category>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var result = await repository.GetByIdAsync(request.CategoryId);
            if (result.IsFailure)
                return result;

            var category = result.Value;
            var newFileName = string.Empty;
            if (request.File is not null)
            {
                var fileName = new Uri(category.ImageUrl).Segments[^1];
                await blobService.DeleteAsync(fileName);
                newFileName = await blobService.UploadFileAsync(request.File, "Category-");
            }

            category.Update(request.CategoryName, request.Description, newFileName);

            return await repository.UpdateAsync(category);
        }
    }
}
