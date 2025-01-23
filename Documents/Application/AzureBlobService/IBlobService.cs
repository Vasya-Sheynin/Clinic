using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Application.AzureBlobService;

public interface IBlobService
{
    Task UploadBlobAsync(string containerName, string blobName, Stream stream, string contentType);
    Task DeleteBlobAsync(string containerName, string blobName);
    Task<FileResponse> ReadBlobAsync(string containerName, string blobName);
    Task UpdateBlobAsync(string containerName, string oldBlobName, string newBlobName, Stream stream, string contentType);
}
