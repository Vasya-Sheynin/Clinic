using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AzureBlobService;

public record BlobServiceOptions
{
    public string? AzureBlobContainerName { get; set; }
}
