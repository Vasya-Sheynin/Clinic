﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AuthService.Exceptions;

public class BadRequestException : Exception
{
    public string Type { get; set; }
    public string Detail { get; set; }
    public string Title { get; set; }

}
