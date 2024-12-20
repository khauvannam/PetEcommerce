﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Share.Domain.Services;

namespace Share.Services;

public class BlobService
{
    private const string ContainerName = "images";
    private readonly BlobServiceClient _client = new(ConnString.BlobConnectionString);

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
