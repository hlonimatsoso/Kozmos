using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kozmos.Api.Controllers
{
    [Route("[controller]")]
    public class ClaimsController : Controller
    {
        //[AllowAnonymous]
        public IActionResult Get()
        {
            var claims = from c in User.Claims
                         select new { c.Type, c.Value };

            return new JsonResult(claims);
        }
    }
}
