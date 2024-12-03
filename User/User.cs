using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Users
{
    public class User : IdentityUser
    {
        public int PhotoId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiry { get; set; }
    }
}
