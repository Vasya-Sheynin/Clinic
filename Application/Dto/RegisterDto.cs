using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public record RegisterDto(
        [Required] string UserName,
        [Required] string Password,
        [Required] string Email,
        [Required] string PhoneNumber
    );
}
