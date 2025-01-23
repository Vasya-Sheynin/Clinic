using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos;

public record FileResponse
{
    public Stream Stream { get; set; }
    public string ContentType { get; set; }
}
