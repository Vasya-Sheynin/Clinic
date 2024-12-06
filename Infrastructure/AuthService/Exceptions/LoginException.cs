using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AuthService.Exceptions;

public class LoginException : BadRequestException
{
    public LoginException()
    {
        Type = "invalid-credentials-exception";
        Title = "Invalid Credentials Exception";
        Detail = "Provided login credentials are not valid.";
    }
}
