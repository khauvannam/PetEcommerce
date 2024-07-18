using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BaseDomain.Results;
using Microsoft.AspNetCore.Http;
using Shared.Domain.Services;

namespace Shared.Services;

public class BlobService
{
    private readonly BlobServiceClient _client = new(Key.BlobConnectionString);
    private const string ContainerName = "images";

    public async Task<string> UploadFileAsync(IFormFile file, string prefix)
    {
        var clientContainer = _client.GetBlobContainerClient(ContainerName);
        await clientContainer.CreateIfNotExistsAsync();
        await clientContainer.SetAccessPolicyAsync(PublicAccessType.Blob);
        var fileName = $"{prefix}{Guid.NewGuid().ToString()}";
        var blobClient = clientContainer.GetBlobClient(fileName);
        await using var data = file.OpenReadStream();

        await blobClient.UploadAsync(data, true);

        return blobClient.Uri.ToString();
    }

    public async Task DeleteAsync(string fileName)
    {
        var clientContainer = _client.GetBlobContainerClient(ContainerName);
        var blobClient = clientContainer.GetBlobClient(fileName);
        await blobClient.DeleteIfExistsAsync();
    }
}
