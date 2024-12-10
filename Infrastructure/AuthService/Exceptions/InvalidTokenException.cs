using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AuthService.Exceptions;

public class InvalidTokenException : BadRequestException
{
    public InvalidTokenException()
    {
        Type = "invalid-token-exception";
        Title = "Invalid Token Exception";
        Detail = "Provided token is not valid.";
    }
}
