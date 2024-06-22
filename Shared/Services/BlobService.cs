using Azure.Storage.Blobs;
using Microsoft.IdentityModel.Tokens;
using Shared.Domain.Results;
using Shared.Domain.Services;

namespace Shared.Services;

public class BlobService
{
    private BlobServiceClient _client = null!;
    private string ContainerName { get; set; } = null!;

    public void DeclareContainerName(string containerName)
    {
        ContainerName = containerName;
        _client = new BlobServiceClient(Key.BlobConnectionString);
    }

    public async Task<Result> UploadFileAsync(Stream fileStream)
    {
        if (ContainerName.IsNullOrEmpty())
        {
            Result.Failure(
                new("BlobService.Error", "Your storage name is nullable, try contact to admin")
            );
        }

        var clientContainer = _client.GetBlobContainerClient(ContainerName);
        await clientContainer.CreateIfNotExistsAsync();
        var fileName = Guid.NewGuid().ToString();
        var client = clientContainer.GetBlobClient(fileName);
        await client.UploadAsync(fileStream, true);
        return Result.Success();
    }
}
