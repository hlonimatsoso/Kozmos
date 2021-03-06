﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Kozmos.BlazorServer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Login()
        {

            return Challenge(new AuthenticationProperties
            {
                RedirectUri = "/"
            }, "oidc");
        }

        [HttpPost]
        public IActionResult Logout()
        {

            return SignOut("cookie","oidc");
           
        }
    }
}
