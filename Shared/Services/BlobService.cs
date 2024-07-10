using Azure.Storage.Blobs;
using Microsoft.IdentityModel.Tokens;
using Shared.Domain.Results;

namespace Shared.Services;

public class BlobService
{
    private readonly BlobServiceClient _client = null!;
    private const string ContainerName = "files";

    public async Task<Result<string>> UploadFileAsync(Stream fileStream)
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
        var fileName = Guid.NewGuid().ToString();
        var blobClient = clientContainer.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream, true);
        return Result.Success(fileName);
    }

    public async Task<Result> DeleteAsync(string fileName)
    {
        var clientContainer = _client.GetBlobContainerClient(ContainerName);
        var blobClient = clientContainer.GetBlobClient(fileName);
        await blobClient.DeleteIfExistsAsync();
        return Result.Success();
    }
}
