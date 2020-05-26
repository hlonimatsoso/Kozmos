using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Kozmos.AspPages
{
    public class HomeController : Controller
    {


        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Login()
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = "/"
            }, "oidc"); ;
        }


        public IActionResult Logout() => SignOut("cookie", "oidc");

        public async Task<IActionResult> CallApiAsUser()
        {
            var client = _httpClientFactory.CreateClient("user_client");

            var response = await client.GetStringAsync("test");
            ViewBag.Json = JArray.Parse(response).ToString();

            return View("CallApi");
        }

        [AllowAnonymous]
        public async Task<IActionResult> CallApiAsClient()
        {
            var client = _httpClientFactory.CreateClient("client");

            var response = await client.GetStringAsync("test");
            ViewBag.Json = JArray.Parse(response).ToString();

            return View("CallApi");
        }
    }
}
