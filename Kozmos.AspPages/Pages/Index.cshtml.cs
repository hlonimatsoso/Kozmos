using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Kozmos.AspPages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string at = string.Empty;
        public string idt = string.Empty;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            throw new Exception("BOOM");

            at = HttpContext.GetTokenAsync("access_token").Result;
            if (!string.IsNullOrWhiteSpace(at))
                _logger.LogInformation("Access token: {@Access_Token} for User '{UserId}'",at,User.Identity.Name);

            idt = HttpContext.GetTokenAsync("id_token").Result;
            if (!string.IsNullOrWhiteSpace(idt))
                _logger.LogInformation("ID token: {@ID_Token}", at);

        }
    }
}
