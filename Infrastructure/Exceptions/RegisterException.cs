using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Exceptions
{
    public class RegisterException : BadRequestException
    {
        public RegisterException(string message)
        {
            Type = "invalid-credentials-exception";
            Title = "Invalid Credentials Exception";
            Detail = message;
        }
    }
}
