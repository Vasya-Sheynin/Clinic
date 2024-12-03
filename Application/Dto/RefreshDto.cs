﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public record RefreshDto(
        [Required] string RefreshToken, 
        [Required] string AccessToken
    );
}