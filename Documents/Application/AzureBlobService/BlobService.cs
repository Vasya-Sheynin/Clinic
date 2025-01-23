using Application.Dtos;
using Azure.Core;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AzureBlobService;

public class BlobService : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task DeleteBlobAsync(string containerName, string blobName)
    {
        var blobClient = await GetBlobClient(containerName, blobName);
        await blobClient.DeleteIfExistsAsync();
    }

    public async Task<FileResponse> ReadBlobAsync(string containerName, string blobName)
    {
        var blobClient = await GetBlobClient(containerName, blobName);
        var result = await blobClient.DownloadContentAsync();

        return new FileResponse { Stream = result.Value.Content.ToStream(), ContentType = result.Value.Details.ContentType };
    }

    public async Task UpdateBlobAsync(string containerName, string oldBlobName, string newBlobName, Stream stream, string contentType)
    {
        await DeleteBlobAsync(containerName, oldBlobName);
        await UploadBlobAsync(containerName, newBlobName, stream, contentType);
    }

    public async Task UploadBlobAsync(string containerName, string blobName, Stream stream, string contentType)
    {
        var blobClient = await GetBlobClient(containerName, blobName);
        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = contentType});
    }

    private async Task<BlobClient> GetBlobClient(string containerName, string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        var blobClient = containerClient.GetBlobClient(blobName);

        return blobClient;
    }
}
