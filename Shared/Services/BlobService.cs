using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Shared.Domain.Results;
using Shared.Domain.Services;

namespace Shared.Services;

public class BlobService
{
    private readonly BlobServiceClient _client = new(Key.BlobConnectionString);
    private const string ContainerName = "files";

    public async Task<Result<string>> UploadFileAsync(IFormFile file, string prefix)
    {
        if (ContainerName.IsNullOrEmpty())
        {
            Result.Failure(
                new(
                    "BlobService.Error",
                    "Maybe the server go down because i dont find any containerName"
                )
            );
        }

        var clientContainer = _client.GetBlobContainerClient(ContainerName);
        await clientContainer.CreateIfNotExistsAsync();
        var fileName = $"{prefix}{Guid.NewGuid().ToString()}";
        var blobClient = clientContainer.GetBlobClient(fileName);
        await using var data = file.OpenReadStream();

        await blobClient.UploadAsync(data, true);

        return Result.Success(blobClient.Uri.ToString());
    }

    public async Task<Result> DeleteAsync(string fileName)
    {
        var clientContainer = _client.GetBlobContainerClient(ContainerName);
        var blobClient = clientContainer.GetBlobClient(fileName);
        await blobClient.DeleteIfExistsAsync();
        return Result.Success();
    }
}
