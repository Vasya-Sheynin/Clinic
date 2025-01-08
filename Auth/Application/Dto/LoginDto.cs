using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto;

public record LoginDto(
    [Required] string UserName,
    [Required] string Password
);
