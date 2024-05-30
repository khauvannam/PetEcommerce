using Azure.Storage.Blobs;
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

    public async Task UploadFileAsync(Stream fileStream)
    {
        var clientContainer = _client.GetBlobContainerClient(ContainerName);
        await clientContainer.CreateIfNotExistsAsync();
        var fileName = Guid.NewGuid().ToString();
        var client = clientContainer.GetBlobClient(fileName);
        await client.UploadAsync(fileStream, true);
    }
}
