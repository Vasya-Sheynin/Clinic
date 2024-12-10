using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Users;

public class User : IdentityUser
{
    public Guid PhotoId { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiry { get; set; }
}
