using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;

namespace Kozmos.AspPages.Pages
{
    public class ApiCallsModel : PageModel
    {

        public string claimString = "not set";

        private readonly IHttpClientFactory _httpClientFactory;

        public ApiCallsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public void OnGet()
        {
            claimString = "From constructor";
        }


        public async Task OnPostAsUser()
        {

            var client = _httpClientFactory.CreateClient("user_client");

            var response = await client.GetStringAsync("claims");

            claimString = JArray.Parse(response).ToString();

            Page();
        }

        public async Task OnPostAsClient()
        {

            var client = _httpClientFactory.CreateClient("client");

            var response = await client.GetStringAsync("claims");

            claimString = JArray.Parse(response).ToString();

            Page();
        }

    }
}