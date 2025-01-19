namespace Infrastructure.AuthService.TokenOptions;

public class AccessTokenOptions
{
    public string ValidIssuer { get; set; }
    public string ValidAudience { get; set; }
    public double ExpirationTime { get; set; }
}
