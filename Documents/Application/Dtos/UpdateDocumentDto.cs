using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos;

public record UpdateDocumentDto
{
    [Required]
    public Guid ResultId { get; set; }

    [Required]
    public IFormFile File { get; set; }
}
