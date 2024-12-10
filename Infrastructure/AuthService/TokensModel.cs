using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AuthService;

public class TokensModel
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
