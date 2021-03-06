﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kozmos.Models
{
    public class KozmosUser : IdentityUser
    {
        public string City { get; set; }
        public bool IsEnabled { get; set; }
        public string EmployeeId { get; set; }
    }
}
